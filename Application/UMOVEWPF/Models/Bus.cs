using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace UMOVEWPF.Models
{
    public enum RouteName
    {
        None, R1A, R10, R11, R13, R132, R133, R137, R139
    }

    public enum BusModel
    {
        MBeCitaro,    // 392 kWh, 1.11 kWh/km
        YutongE12,    // 422 kWh, 0.84 kWh/km
        BYDK9,        // 324 kWh, 1.26 kWh/km
        Volvo7900E    // 470 kWh, 1.00 kWh/km
    }

    public class Bus : INotifyPropertyChanged
    {
        private string _busId;
        public string BusId
        {
            get => _busId;
            set { _busId = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); }
        }

        private BusModel _model;
        public BusModel Model
        {
            get => _model;
            set 
            { 
                _model = value;
                // Update battery capacity and consumption based on model
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
        public string Year
        {
            get => _year;
            set { _year = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); }
        }

        private double _batteryCapacity;
        public double BatteryCapacity
        {
            get => _batteryCapacity;
            set { _batteryCapacity = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); }
        }

        private double _batteryLevel;
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
        public double Consumption
        {
            get => _consumption;
            set { _consumption = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); }
        }

        private DateTime _lastUpdate;
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
        public DateTime StatusChangedAt
        {
            get => _statusChangedAt;
            set { _statusChangedAt = value; OnPropertyChanged(); }
        }

        private BusStatus _status;
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
        public RouteName Route
        {
            get => _route;
            set { _route = value; OnPropertyChanged(); }
        }

        public BatteryInfo BatteryInfo { get; set; } = new BatteryInfo();

        public bool IsCritical => BatteryLevel < 20;

        public bool IsInService => Status == BusStatus.Inroute || Status == BusStatus.Intercept || Status == BusStatus.Returning;

        public string DisplayText => $"{BusId} | {Model} | {BatteryLevel:F1}%";

        public Bus ReplacingBus { get; set; }

        public double TimeInCurrentStatus { get; set; }

        // Beregn tid til 13% batteri tilbage ved 20 km/t
        public TimeSpan TimeLeftUntil13Percent(double averageSpeedKmh = 20)
        {
            double percentToUse = BatteryLevel - 13;
            if (percentToUse <= 0 || Consumption <= 0) return TimeSpan.Zero;
            double kmLeft = (percentToUse / 100.0) * BatteryCapacity / Consumption;
            double hoursLeft = kmLeft / averageSpeedKmh;
            return TimeSpan.FromHours(hoursLeft);
        }

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}