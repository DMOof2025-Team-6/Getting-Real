using System.Collections.ObjectModel;
using System.ComponentModel;
using UMOVEWPF.Models;
using System;
using System.Linq;

namespace UMOVEWPF.ViewModels
{
    /// <summary>
    /// ViewModel til tilføjelse og redigering af busser
    /// Denne klasse implementerer INotifyPropertyChanged for at understøtte databinding i UI
    /// </summary>
    public class AddEditBusViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Bussen der redigeres eller tilføjes
        /// </summary>
        public Bus Bus { get; set; }

        /// <summary>
        /// Samling af tilgængelige ruter
        /// </summary>
        public ObservableCollection<RouteName> Routes { get; }

        /// <summary>
        /// Samling af tilgængelige busmodeller
        /// </summary>
        public ObservableCollection<BusModel> Models { get; }

        /// <summary>
        /// Opretter en ny AddEditBusViewModel
        /// </summary>
        /// <param name="bus">Den eksisterende bus der skal redigeres, eller null hvis der oprettes en ny</param>
        public AddEditBusViewModel(Bus bus = null)
        {
            Bus = bus ?? new Bus { Model = BusModel.MBeCitaro, BatteryLevel = 100, Status = BusStatus.Garage };
            Routes = new ObservableCollection<RouteName>(Enum.GetValues(typeof(RouteName)).Cast<RouteName>().Where(r => r != RouteName.None));
            Models = new ObservableCollection<BusModel>(Enum.GetValues(typeof(BusModel)).Cast<BusModel>());
        }

        /// <summary>
        /// Event der udløses når en egenskab ændres
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Hjælpemetode til at udløse PropertyChanged eventet
        /// </summary>
        /// <param name="name">Navnet på den ændrede egenskab</param>
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
} 