using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using UMOVEWPF.Helpers;
using UMOVEWPF.Models;

namespace UMOVEWPF.ViewModels
{
    /// <summary>
    /// ViewModel til visning og redigering af detaljer for en bus
    /// Denne klasse arver fra BaseViewModel for at understøtte databinding i UI
    /// </summary>
    public class BusDetailViewModel : BaseViewModel
    {
        private Bus _bus;
        /// <summary>
        /// Bussen hvis detaljer vises og redigeres
        /// </summary>
        public Bus Bus
        {
            get => _bus;
            set => SetProperty(ref _bus, value);
        }

        private ObservableCollection<Route> _availableRoutes;
        /// <summary>
        /// Samling af tilgængelige ruter som bussen kan tildeles
        /// </summary>
        public ObservableCollection<Route> AvailableRoutes
        {
            get => _availableRoutes;
            set => SetProperty(ref _availableRoutes, value);
        }

        /// <summary>
        /// Kommando til at gemme ændringer
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Kommando til at annullere ændringer
        /// </summary>
        public ICommand CancelCommand { get; }

        /// <summary>
        /// Opretter en ny BusDetailViewModel
        /// </summary>
        /// <param name="bus">Bussen hvis detaljer skal vises, eller null hvis der oprettes en ny</param>
        public BusDetailViewModel(Bus bus = null)
        {
            Bus = bus ?? new Bus();
            AvailableRoutes = new ObservableCollection<Route>();
            SaveCommand = new RelayCommand(async _ => await SaveBusAsync());
            CancelCommand = new RelayCommand(_ => Cancel());

            LoadRoutesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Indlæser tilgængelige ruter fra fil
        /// </summary>
        private async Task LoadRoutesAsync()
        {
            var routes = await FileHelper.LoadRoutesAsync();
            AvailableRoutes.Clear();
            foreach (var route in routes)
            {
                AvailableRoutes.Add(route);
            }
        }

        /// <summary>
        /// Gemmer ændringer til bussen
        /// </summary>
        private async Task SaveBusAsync()
        {
            if (Bus != null)
            {
                // Update the bus's last update time
                Bus.LastUpdate = DateTime.Now;
                
                // Save the bus
                await FileHelper.SaveBusesAsync(new[] { Bus });
            }
        }

        /// <summary>
        /// Annullerer ændringer og lukker vinduet
        /// </summary>
        private void Cancel()
        {
            // This will be handled by the view to close the window
        }
    }
} 