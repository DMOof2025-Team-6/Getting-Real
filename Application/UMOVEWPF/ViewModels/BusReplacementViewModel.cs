using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using UMOVEWPF.Models;

namespace UMOVEWPF.ViewModels
{
    /// <summary>
    /// ViewModel til håndtering af buserstatning når en bus har lavt batteriniveau
    /// Denne klasse implementerer INotifyPropertyChanged for at understøtte databinding i UI
    /// </summary>
    public class BusReplacementViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<Bus> _allBuses;
        private readonly Bus _lowBatteryBus;
        private Bus _selectedReplacementBus;
        private bool _canPostpone = true;
        private int _postponeCount = 0;

        /// <summary>
        /// Bussen med lavt batteriniveau der skal erstattes
        /// </summary>
        public Bus LowBatteryBus => _lowBatteryBus;

        /// <summary>
        /// Samling af tilgængelige busser der kan erstatte den nuværende bus
        /// </summary>
        public ObservableCollection<Bus> AvailableBuses { get; }

        /// <summary>
        /// Den valgte erstatningsbus
        /// </summary>
        public Bus SelectedReplacementBus
        {
            get => _selectedReplacementBus;
            set
            {
                _selectedReplacementBus = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Angiver om udskydelse af erstatning er mulig
        /// </summary>
        public bool CanPostpone
        {
            get => _canPostpone;
            set
            {
                _canPostpone = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Kommando til at vælge en erstatningsbus
        /// </summary>
        public ICommand SelectBusCommand { get; }

        /// <summary>
        /// Kommando til at udskyde erstatningen
        /// </summary>
        public ICommand PostponeCommand { get; }

        /// <summary>
        /// Kommando til at annullere erstatningen
        /// </summary>
        public ICommand CancelCommand { get; }

        /// <summary>
        /// Event der udløses når en erstatningsbus er valgt
        /// </summary>
        public event EventHandler<Bus> BusSelected;

        /// <summary>
        /// Event der udløses når erstatningen udskydes
        /// </summary>
        public event EventHandler Postponed;

        /// <summary>
        /// Event der udløses når erstatningen annulleres
        /// </summary>
        public event EventHandler Cancelled;

        /// <summary>
        /// Opretter en ny BusReplacementViewModel
        /// </summary>
        /// <param name="allBuses">Samling af alle busser i systemet</param>
        /// <param name="lowBatteryBus">Bussen med lavt batteriniveau der skal erstattes</param>
        public BusReplacementViewModel(ObservableCollection<Bus> allBuses, Bus lowBatteryBus)
        {
            _allBuses = allBuses;
            _lowBatteryBus = lowBatteryBus;
            AvailableBuses = new ObservableCollection<Bus>(
                allBuses.Where(b => b.Status == BusStatus.Garage && b.BatteryLevel >= 50)
            );

            SelectBusCommand = new RelayCommand(_ => OnBusSelected());
            PostponeCommand = new RelayCommand(_ => OnPostponed());
            CancelCommand = new RelayCommand(_ => OnCancelled());
        }

        /// <summary>
        /// Håndterer valg af erstatningsbus
        /// </summary>
        private void OnBusSelected()
        {
            if (SelectedReplacementBus == null) return;

            // Set the replacement bus to intercept status
            SelectedReplacementBus.Status = BusStatus.Intercept;
            SelectedReplacementBus.Route = _lowBatteryBus.Route;

            // Set the low battery bus to returning status
            _lowBatteryBus.Status = BusStatus.Returning;

            BusSelected?.Invoke(this, SelectedReplacementBus);
        }

        /// <summary>
        /// Håndterer udskydelse af erstatning
        /// </summary>
        private void OnPostponed()
        {
            _postponeCount++;
            if (_postponeCount >= 2)
            {
                CanPostpone = false;
            }
            Postponed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Håndterer annullering af erstatning
        /// </summary>
        private void OnCancelled()
        {
            Cancelled?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Event der udløses når en egenskab ændres
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Hjælpemetode til at udløse PropertyChanged eventet
        /// </summary>
        /// <param name="propertyName">Navnet på den ændrede egenskab</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 