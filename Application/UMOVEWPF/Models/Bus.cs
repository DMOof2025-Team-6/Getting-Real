using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace UMOVEWPF.Models
{
    /// <summary>
    /// Enumeration af tilgængelige busruter
    /// </summary>
    public enum RouteName
    {
        None, R1A, R10, R11, R13, R132, R133, R137, R139
    }

    /// <summary>
    /// Enumeration af tilgængelige busmodeller med deres specifikationer
    /// </summary>
    public enum BusModel
    {
        MBeCitaro,    // 392 kWh, 1.11 kWh/km
        YutongE12,    // 422 kWh, 0.84 kWh/km
        BYDK9,        // 324 kWh, 1.26 kWh/km
        Volvo7900E    // 470 kWh, 1.00 kWh/km
    }

    /// <summary>
    /// Repræsenterer en bus i systemet
    /// Denne klasse implementerer INotifyPropertyChanged for at understøtte databinding i UI
    /// </summary>
    public class Bus : INotifyPropertyChanged
    {
        private string _busId;
        /// <summary>
        /// Bussens unikke identifikationsnummer
        /// </summary>
        public string BusId
        {
            get => _busId;
            set { _busId = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); }
        }

        private BusModel _model;
        /// <summary>
        /// Bussens model
        /// Ved ændring opdateres batterikapacitet og forbrug automatisk
        /// </summary>
        public BusModel Model
        {
            get => _model;
            set 
            { 
                _model = value;
                // Opdater batterikapacitet og forbrug baseret på model
                switch (value)
                {
                    case BusModel.MBeCitaro:
                        BatteryCapacity = 392;
                        Consumption = 1.11;
                        break;
                    case BusModel.YutongE12:
                        BatteryCapacity = 422;
                        Consumption = 0.84;
                        break;
                    case BusModel.BYDK9:
                        BatteryCapacity = 324;
                        Consumption = 1.26;
                        break;
                    case BusModel.Volvo7900E:
                        BatteryCapacity = 470;
                        Consumption = 1.00;
                        break;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayText));
            }
        }

        private string _year;
        /// <summary>
        /// Bussens årgang
        /// </summary>
        public string Year
        {
            get => _year;
            set { _year = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); }
        }

        private double _batteryCapacity;
        /// <summary>
        /// Bussens batterikapacitet i kWh
        /// </summary>
        public double BatteryCapacity
        {
            get => _batteryCapacity;
            set { _batteryCapacity = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); }
        }

        private double _batteryLevel;
        /// <summary>
        /// Bussens nuværende batteriniveau i procent
        /// </summary>
        public double BatteryLevel
        {
            get => _batteryLevel;
            set
            {
                if (_batteryLevel != value)
                {
                    _batteryLevel = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DisplayText));
                    OnPropertyChanged(nameof(Status));
                    OnPropertyChanged(nameof(IsCritical));
                    OnPropertyChanged(nameof(TimeLeftUntil13PercentFormatted));
                    BatteryInfo.Level = value;
                }
            }
        }

        private double _consumption;
        /// <summary>
        /// Bussens batteriforbrug i kWh/km
        /// </summary>
        public double Consumption
        {
            get => _consumption;
            set { _consumption = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); }
        }

        private DateTime _lastUpdate;
        /// <summary>
        /// Tidspunkt for sidste opdatering af busdata
        /// </summary>
        public DateTime LastUpdate
        {
            get => _lastUpdate;
            set
            {
                _lastUpdate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayText));
                OnPropertyChanged(nameof(TimeLeftUntil13PercentFormatted));
            }
        }

        private DateTime _statusChangedAt = DateTime.Now;
        /// <summary>
        /// Tidspunkt for sidste ændring af busstatus
        /// </summary>
        public DateTime StatusChangedAt
        {
            get => _statusChangedAt;
            set { _statusChangedAt = value; OnPropertyChanged(); }
        }

        private BusStatus _status;
        /// <summary>
        /// Bussens nuværende status
        /// </summary>
        public BusStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    StatusChangedAt = DateTime.Now;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsInService));
                }
            }
        }

        private RouteName _route;
        /// <summary>
        /// Ruten som bussen kører på
        /// </summary>
        public RouteName Route
        {
            get => _route;
            set { _route = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Information om bussens batteri
        /// </summary>
        public BatteryInfo BatteryInfo { get; set; } = new BatteryInfo();

        /// <summary>
        /// Angiver om bussen har kritisk lavt batteriniveau (under 20%)
        /// </summary>
        public bool IsCritical => BatteryLevel < 20;

        /// <summary>
        /// Angiver om bussen er i drift (på rute, på vej til erstatning, eller på vej tilbage)
        /// </summary>
        public bool IsInService => Status == BusStatus.Inroute || Status == BusStatus.Intercept || Status == BusStatus.Returning;

        /// <summary>
        /// Tekst der vises i UI med businformation
        /// </summary>
        public string DisplayText => $"{BusId} | {Model} | {BatteryLevel:F1}%";

        /// <summary>
        /// Referencen til den bus som denne bus erstatter
        /// </summary>
        public Bus ReplacingBus { get; set; }

        /// <summary>
        /// Tid i minutter som bussen har været i nuværende status
        /// </summary>
        public double TimeInCurrentStatus { get; set; }

        /// <summary>
        /// Beregner tid tilbage før batteriet når 13% ved en given gennemsnitshastighed
        /// </summary>
        /// <param name="averageSpeedKmh">Gennemsnitshastighed i km/t</param>
        /// <returns>Tid tilbage før batteriet når 13%</returns>
        public TimeSpan TimeLeftUntil13Percent(double averageSpeedKmh = 20)
        {
            double percentToUse = BatteryLevel - 13;
            if (percentToUse <= 0 || Consumption <= 0) return TimeSpan.Zero;
            double kmLeft = (percentToUse / 100.0) * BatteryCapacity / Consumption;
            double hoursLeft = kmLeft / averageSpeedKmh;
            return TimeSpan.FromHours(hoursLeft);
        }

        /// <summary>
        /// Formateret streng der viser tid tilbage før batteriet når 13%
        /// </summary>
        public string TimeLeftUntil13PercentFormatted
        {
            get
            {
                var t = TimeLeftUntil13Percent();
                if (t.TotalSeconds <= 0)
                    return "0:00:00";
                return $"{(int)t.TotalHours}:{t.Minutes:D2}:{t.Seconds:D2}";
            }
        }

        /// <summary>
        /// Event der udløses når en egenskab ændres
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Hjælpemetode til at udløse PropertyChanged eventet
        /// </summary>
        /// <param name="name">Navnet på den ændrede egenskab</param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}