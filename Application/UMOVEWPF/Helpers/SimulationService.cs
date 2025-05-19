using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using UMOVEWPF.Models;

namespace UMOVEWPF.Helpers
{
    public class SimulationService
    {
        private readonly DispatcherTimer _timer;
        private readonly ObservableCollection<Bus> _buses;
        private readonly double _averageSpeedKmh;
        private readonly Weather _weather;

        public bool IsRunning => _timer.IsEnabled;

        public SimulationService(ObservableCollection<Bus> buses, Weather weather, double averageSpeedKmh = 20)
        {
            _buses = buses;
            _weather = weather;
            _averageSpeedKmh = averageSpeedKmh;
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (s, e) => SimulateTick();
        }

        public void Start() => _timer.Start();
        public void Stop() => _timer.Stop();

        private void SimulateTick()
        {
            double hours = 1.0 / 3600.0; // 1 sekund
            double consumptionMultiplier = _weather?.ConsumptionMultiplier ?? 1.0;
            foreach (var bus in _buses)
            {
                if (bus.Status == BusStatus.Inroute || bus.Status == BusStatus.Intercept || bus.Status == BusStatus.Returning)
                {
                    double distance = _averageSpeedKmh * hours;
                    double consumptionKWh = distance * bus.Consumption * consumptionMultiplier;
                    double percentUsed = (consumptionKWh / bus.BatteryCapacity) * 100.0;
                    bus.BatteryLevel -= percentUsed;
                    if (bus.BatteryLevel < 0) bus.BatteryLevel = 0;
                    bus.LastUpdate = bus.LastUpdate.AddSeconds(1);
                }
            }
        }
    }
} 