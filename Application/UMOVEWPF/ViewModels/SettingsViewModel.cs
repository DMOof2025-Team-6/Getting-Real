using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using UMOVEWPF.Helpers;
using UMOVEWPF.Models;

namespace UMOVEWPF.ViewModels
{
    /// <summary>
    /// ViewModel til håndtering af systemindstillinger
    /// Denne klasse arver fra BaseViewModel for at understøtte databinding i UI
    /// </summary>
    public class SettingsViewModel : BaseViewModel
    {
        private ObservableCollection<Route> _routes;
        /// <summary>
        /// Samling af ruter i systemet
        /// </summary>
        public ObservableCollection<Route> Routes
        {
            get => _routes;
            set => SetProperty(ref _routes, value);
        }

        private Route _selectedRoute;
        /// <summary>
        /// Den valgte rute i UI
        /// </summary>
        public Route SelectedRoute
        {
            get => _selectedRoute;
            set => SetProperty(ref _selectedRoute, value);
        }

        /// <summary>
        /// Kommando til at tilføje en ny rute
        /// </summary>
        public ICommand AddRouteCommand { get; }

        /// <summary>
        /// Kommando til at redigere en eksisterende rute
        /// </summary>
        public ICommand EditRouteCommand { get; }

        /// <summary>
        /// Kommando til at slette en rute
        /// </summary>
        public ICommand DeleteRouteCommand { get; }

        /// <summary>
        /// Kommando til at gemme ændringer
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Opretter en ny SettingsViewModel
        /// </summary>
        public SettingsViewModel()
        {
            Routes = new ObservableCollection<Route>();
            AddRouteCommand = new RelayCommand(_ => AddRoute());
            EditRouteCommand = new RelayCommand(_ => EditRoute(), _ => SelectedRoute != null);
            DeleteRouteCommand = new RelayCommand(_ => DeleteRoute(), _ => SelectedRoute != null);
            SaveCommand = new RelayCommand(async _ => await SaveRoutesAsync());

            LoadRoutesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Indlæser ruter fra fil
        /// </summary>
        private async Task LoadRoutesAsync()
        {
            var routes = await FileHelper.LoadRoutesAsync();
            Routes.Clear();
            foreach (var route in routes)
            {
                Routes.Add(route);
            }
        }

        /// <summary>
        /// Tilføjer en ny rute til systemet
        /// </summary>
        private void AddRoute()
        {
            var newRoute = new Route
            {
                Name = RouteName.None,
                Description = "New Route",
                Distance = 0,
                EstimatedTime = 0
            };
            Routes.Add(newRoute);
            SelectedRoute = newRoute;
        }

        /// <summary>
        /// Forbereder redigering af en rute
        /// </summary>
        private void EditRoute()
        {
            // This will be handled by the view
        }

        /// <summary>
        /// Sletter den valgte rute fra systemet
        /// </summary>
        private void DeleteRoute()
        {
            if (SelectedRoute != null)
            {
                Routes.Remove(SelectedRoute);
            }
        }

        /// <summary>
        /// Gemmer alle ruter til fil
        /// </summary>
        private async Task SaveRoutesAsync()
        {
            await FileHelper.SaveRoutesAsync(Routes);
        }
    }
} 