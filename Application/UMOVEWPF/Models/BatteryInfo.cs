using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UMOVEWPF.Models
{
    /// <summary>
    /// Repræsenterer information om et batteri
    /// Denne klasse implementerer INotifyPropertyChanged for at understøtte databinding i UI
    /// </summary>
    public class BatteryInfo : INotifyPropertyChanged
    {
        private double _capacity;
        /// <summary>
        /// Batteriets kapacitet i kWh
        /// </summary>
        public double Capacity
        {
            get => _capacity;
            set { _capacity = value; OnPropertyChanged(); }
        }

        private double _level;
        /// <summary>
        /// Batteriets nuværende niveau i procent
        /// </summary>
        public double Level
        {
            get => _level;
            set 
            { 
                _level = value; 
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCritical));
            }
        }

        private double _consumption;
        /// <summary>
        /// Batteriets forbrug i kWh/km
        /// </summary>
        public double Consumption
        {
            get => _consumption;
            set { _consumption = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Angiver om batteriet har kritisk lavt niveau (under 20%)
        /// </summary>
        public bool IsCritical => Level < 20;

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