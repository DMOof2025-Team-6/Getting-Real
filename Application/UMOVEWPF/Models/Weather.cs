using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;

namespace UMOVEWPF.Models
{
    /// <summary>
    /// Repræsenterer vejrdata og deres påvirkning på busbatteriforbrug
    /// Denne klasse implementerer INotifyPropertyChanged for at understøtte databinding i UI
    /// </summary>
    public class Weather : INotifyPropertyChanged
    {
        /// <summary>
        /// Enumeration af måneder for vejrdata
        /// </summary>
        public enum Month
        {
            Januar,
            Februar,
            Marts,
            April,
            Maj,
            Juni,
            Juli,
            August,
            September,
            Oktober,
            November,
            December
        }

        private Month _selectedMonth = Month.Januar;
        /// <summary>
        /// Den valgte måned for vejrdata
        /// </summary>
        public Month SelectedMonth
        {
            get => _selectedMonth;
            set { _selectedMonth = value; OnPropertyChanged(nameof(SelectedMonth)); OnPropertyChanged(nameof(ConsumptionMultiplier)); }
        }

        private bool _isRaining;
        /// <summary>
        /// Angiver om det regner
        /// Påvirker batteriforbruget med en ekstra faktor
        /// </summary>
        public bool IsRaining
        {
            get => _isRaining;
            set { _isRaining = value; OnPropertyChanged(nameof(IsRaining)); OnPropertyChanged(nameof(ConsumptionMultiplier)); }
        }

        /// <summary>
        /// Faktor der påvirker batteriforbruget baseret på vejrforhold
        /// </summary>
        public double ConsumptionMultiplier => GetConsumptionMultiplier();

        /// <summary>
        /// Beregner forbrugsmultiplikatoren baseret på måned og regn
        /// </summary>
        /// <returns>En multiplikator der påvirker batteriforbruget</returns>
        public double GetConsumptionMultiplier()
        {
            double baseMultiplier = 1.0;
            switch (SelectedMonth)
            {
                case Month.December:
                case Month.Januar:
                case Month.Februar:
                    baseMultiplier = 1.4; // Vinter: 40% højere forbrug
                    break;
                case Month.Marts:
                case Month.November:
                    baseMultiplier = 1.2; // Sen efterår/tidlig forår: 20% højere forbrug
                    break;
                case Month.April:
                case Month.Oktober:
                    baseMultiplier = 1.0; // Normal forbrug
                    break;
                case Month.Maj:
                case Month.September:
                    baseMultiplier = 0.9; // 10% lavere forbrug
                    break;
                case Month.Juni:
                case Month.Juli:
                case Month.August:
                    baseMultiplier = 0.8; // Sommer: 20% lavere forbrug
                    break;
            }
            if (IsRaining)
                baseMultiplier += 0.1; // Regn: 10% ekstra forbrug
            return baseMultiplier;
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

        /// <summary>
        /// Gemmer vejrdata til en fil
        /// </summary>
        /// <param name="path">Stien til filen der skal gemmes til</param>
        public void SaveToFile(string path)
        {
            using (var sw = new StreamWriter(path, false))
            {
                sw.WriteLine($"{SelectedMonth};{IsRaining}");
            }
        }

        /// <summary>
        /// Indlæser vejrdata fra en fil
        /// </summary>
        /// <param name="path">Stien til filen der skal indlæses fra</param>
        public void LoadFromFile(string path)
        {
            if (!File.Exists(path)) return;
            using (var sr = new StreamReader(path))
            {
                var line = sr.ReadLine();
                if (line != null)
                {
                    var parts = line.Split(';');
                    if (parts.Length == 2)
                    {
                        if (Enum.TryParse(parts[0], out Month month))
                            SelectedMonth = month;
                        if (bool.TryParse(parts[1], out bool rain))
                            IsRaining = rain;
                    }
                }
            }
        }
    }
}
