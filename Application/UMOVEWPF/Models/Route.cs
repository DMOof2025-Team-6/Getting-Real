using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UMOVEWPF.Models
{
    /// <summary>
    /// Repræsenterer en busrute i systemet
    /// Denne klasse implementerer INotifyPropertyChanged for at understøtte databinding i UI
    /// </summary>
    public class Route : INotifyPropertyChanged
    {
        private RouteName _name;
        /// <summary>
        /// Navnet på ruten
        /// </summary>
        public RouteName Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        private string _description;
        /// <summary>
        /// Beskrivelse af ruten
        /// </summary>
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        private double _distance;
        /// <summary>
        /// Ruttens længde i kilometer
        /// </summary>
        public double Distance
        {
            get => _distance;
            set { _distance = value; OnPropertyChanged(); }
        }

        private int _estimatedTime;
        /// <summary>
        /// Estimeret køretid i minutter
        /// </summary>
        public int EstimatedTime
        {
            get => _estimatedTime;
            set { _estimatedTime = value; OnPropertyChanged(); }
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