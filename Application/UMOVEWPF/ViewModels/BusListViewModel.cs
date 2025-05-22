using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using UMOVEWPF.Helpers;
using UMOVEWPF.Models;
using System;
using System.Linq;
using UMOVEWPF; // For SimulationService

namespace UMOVEWPF.ViewModels
{
    /// <summary>
    /// ViewModel til håndtering af listen over busser
    /// Denne klasse arver fra BaseViewModel for at understøtte databinding i UI
    /// </summary>
    public class BusListViewModel : BaseViewModel
    {
        private ObservableCollection<Bus> _buses;
        /// <summary>
        /// Samling af alle busser i systemet
        /// </summary>
        public ObservableCollection<Bus> Buses
        {
            get => _buses;
            set => SetProperty(ref _buses, value);
        }

        private Bus _selectedBus;
        /// <summary>
        /// Den valgte bus i UI
        /// </summary>
        public Bus SelectedBus
        {
            get => _selectedBus;
            set => SetProperty(ref _selectedBus, value);
        }

        private string _batteryLevelInput;
        /// <summary>
        /// Input til opdatering af batteriniveau
        /// </summary>
        public string BatteryLevelInput
        {
            get => _batteryLevelInput;
            set => SetProperty(ref _batteryLevelInput, value);
        }

        /// <summary>
        /// Kommando til at opdatere listen over busser
        /// </summary>
        public ICommand RefreshCommand { get; }

        /// <summary>
        /// Kommando til at tilføje en ny bus
        /// </summary>
        public ICommand AddBusCommand { get; }

        /// <summary>
        /// Kommando til at redigere en eksisterende bus
        /// </summary>
        public ICommand EditBusCommand { get; }

        /// <summary>
        /// Kommando til at slette en bus
        /// </summary>
        public ICommand DeleteBusCommand { get; }

        /// <summary>
        /// Kommando til at opdatere batteristatus for alle busser
        /// </summary>
        public ICommand UpdateBatteryStatusCommand { get; }

        /// <summary>
        /// Kommando til at opdatere batteriniveau for den valgte bus
        /// </summary>
        public ICommand UpdateBatteryLevelCommand { get; }

        private SimulationService _simService;
        private Weather _weather = new Weather();

        /// <summary>
        /// Opretter en ny BusListViewModel
        /// </summary>
        public BusListViewModel()
        {
            Buses = new ObservableCollection<Bus>();
            RefreshCommand = new RelayCommand(async _ => await LoadBusesAsync());
            AddBusCommand = new RelayCommand(_ => AddBus());
            EditBusCommand = new RelayCommand(_ => EditBus(), _ => SelectedBus != null);
            DeleteBusCommand = new RelayCommand(_ => DeleteBus(), _ => SelectedBus != null);
            UpdateBatteryStatusCommand = new RelayCommand(_ => UpdateBatteryStatus());
            UpdateBatteryLevelCommand = new RelayCommand(_ => UpdateBatteryLevel(), _ => SelectedBus != null && double.TryParse(BatteryLevelInput, out double _));
            _simService = new SimulationService(Buses, _weather);
            _simService.Start(); // Start live simulering automatisk
            // Fjern synkron indlæsning af busser her!
        }

        /// <summary>
        /// Initialiserer ViewModel ved at indlæse busser
        /// </summary>
        public async Task InitializeAsync()
        {
            await LoadBusesAsync();
        }

        /// <summary>
        /// Indlæser busser fra fil og opdaterer listen
        /// </summary>
        private async Task LoadBusesAsync()
        {
            var loadedBuses = await FileHelper.LoadBusesAsync();

            // Fjern busser der ikke længere findes
            for (int i = Buses.Count - 1; i >= 0; i--)
            {
                var bus = Buses[i];
                if (!loadedBuses.Any(b => b.BusId == bus.BusId))
                    Buses.RemoveAt(i);
            }

            // Opdater eksisterende og tilføj nye
            foreach (var loadedBus in loadedBuses)
            {
                var existing = Buses.FirstOrDefault(b => b.BusId == loadedBus.BusId);
                if (existing != null)
                {
                    // Opdater alle properties
                    existing.Year = loadedBus.Year;
                    existing.BatteryCapacity = loadedBus.BatteryCapacity;
                    existing.Consumption = loadedBus.Consumption;
                    existing.Route = loadedBus.Route;
                    existing.BatteryLevel = loadedBus.BatteryLevel;
                    existing.Status = loadedBus.Status;
                    existing.LastUpdate = loadedBus.LastUpdate;
                }
                else
                {
                    Buses.Add(loadedBus);
                }
            }
        }

        /// <summary>
        /// Tilføjer en ny bus til systemet
        /// </summary>
        private void AddBus()
        {
            var newBus = new Bus
            {
                BusId = "New Bus",
                Model = BusModel.MBeCitaro,
                Year = DateTime.Now.Year.ToString(),
                Status = BusStatus.Free
            };
            Buses.Add(newBus);
            SelectedBus = newBus;
        }

        /// <summary>
        /// Forbereder redigering af en bus
        /// </summary>
        private void EditBus()
        {
            // This will be handled by the BusDetailViewModel
        }

        /// <summary>
        /// Sletter den valgte bus fra systemet
        /// </summary>
        private async void DeleteBus()
        {
            if (SelectedBus != null)
            {
                Buses.Remove(SelectedBus);
                await FileHelper.SaveBusesAsync(Buses);
            }
        }

        /// <summary>
        /// Opdaterer batteriniveauet for den valgte bus
        /// </summary>
        private void UpdateBatteryLevel()
        {
            if (SelectedBus == null) return;
            if (double.TryParse(BatteryLevelInput, out double newLevel))
            {
                if (newLevel < 0) newLevel = 0;
                if (newLevel > 100) newLevel = 100;
                
                // Update both BatteryLevel and BatteryInfo.Level to ensure all views update
                SelectedBus.BatteryLevel = newLevel;
                if (SelectedBus.BatteryInfo != null)
                {
                    SelectedBus.BatteryInfo.Level = newLevel;
                }
                SelectedBus.LastUpdate = DateTime.Now;
                
                if (newLevel < 30)
                {
                    System.Windows.MessageBox.Show($"Advarsel: {SelectedBus.BusId} har lavt batteriniveau ({newLevel:F1}%)!", "Lavt batteri", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                }
                BatteryLevelInput = string.Empty;
                // Gem busser hvis nødvendigt
                FileHelper.SaveBusesAsync(Buses);
            }
        }

        private void UpdateBatteryStatus()
        {
            // Simuler 30 minutters kørsel for hver bus i drift
            double averageSpeedKmh = 20;
            double hours = 0.5; // 30 minutter
            foreach (var bus in Buses)
            {
                if (bus.Status == BusStatus.Inroute || bus.Status == BusStatus.Intercept || bus.Status == BusStatus.Returning)
                {
                    double distance = averageSpeedKmh * hours; // km
                    double consumptionKWh = distance * bus.Consumption; // kWh brugt
                    double percentUsed = (consumptionKWh / bus.BatteryCapacity) * 100.0;
                    bus.BatteryLevel -= percentUsed;
                    if (bus.BatteryLevel < 0) bus.BatteryLevel = 0;
                    bus.LastUpdate = DateTime.Now;
                    if (bus.BatteryLevel < 30)
                    {
                        System.Windows.MessageBox.Show($"Advarsel: {bus.BusId} har lavt batteriniveau ({bus.BatteryLevel:F1}%)!", "Lavt batteri", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    }
                }
            }
        }
    }
} 