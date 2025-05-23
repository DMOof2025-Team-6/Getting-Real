using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using UMOVEWPF.Models;

namespace UMOVEWPF.ViewModels
{
    /// <summary>
    /// ViewModel til håndtering af vejrdata i UI
    /// Denne klasse implementerer INotifyPropertyChanged for at understøtte databinding i UI
    /// </summary>
    public class WeatherViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Vejrdata modellen
        /// </summary>
        public Weather Weather { get; }

        /// <summary>
        /// Samling af tilgængelige måneder
        /// </summary>
        public ObservableCollection<Weather.Month> Months { get; }

        /// <summary>
        /// Den valgte måned
        /// </summary>
        public Weather.Month SelectedMonth
        {
            get => Weather.SelectedMonth;
            set { Weather.SelectedMonth = value; OnPropertyChanged(nameof(SelectedMonth)); }
        }

        /// <summary>
        /// Angiver om det regner
        /// </summary>
        public bool IsRaining
        {
            get => Weather.IsRaining;
            set { Weather.IsRaining = value; OnPropertyChanged(nameof(IsRaining)); }
        }

        /// <summary>
        /// Kommando der udføres når OK knappen klikkes
        /// </summary>
        public ICommand OkCommand { get; }

        /// <summary>
        /// Event der udløses når OK knappen klikkes
        /// </summary>
        public event EventHandler OkClicked;

        /// <summary>
        /// Opretter en ny WeatherViewModel
        /// </summary>
        /// <param name="weather">Vejrdata modellen der skal bruges</param>
        public WeatherViewModel(Weather weather)
        {
            Weather = weather;
            Months = new ObservableCollection<Weather.Month>(Enum.GetValues(typeof(Weather.Month)).Cast<Weather.Month>());
            OkCommand = new RelayCommand(_ => {
                Weather.SaveToFile("weather.json");
                OkClicked?.Invoke(this, EventArgs.Empty);
            });
        }

        /// <summary>
        /// Event der udløses når en egenskab ændres
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Hjælpemetode til at udløse PropertyChanged eventet
        /// </summary>
        /// <param name="propertyName">Navnet på den ændrede egenskab</param>
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
} 