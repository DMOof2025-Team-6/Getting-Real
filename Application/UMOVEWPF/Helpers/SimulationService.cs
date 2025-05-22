using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using UMOVEWPF.Models;
using System.Collections.Generic;
using System.Linq;

namespace UMOVEWPF.Helpers
{
    /// <summary>
    /// Service til at simulere busdrift og batteristatus
    /// Denne klasse håndterer den kontinuerlige simulation af bussers bevægelse, batteriforbrug og opladning
    /// </summary>
    public class SimulationService
    {
        private readonly DispatcherTimer _timer;
        private readonly ObservableCollection<Bus> _buses;
        private readonly double _averageSpeedKmh;
        private readonly Weather _weather;
        private DateTime _lastTick;
        private HashSet<Bus> _warnedBuses = new HashSet<Bus>();

        /// <summary>
        /// Angiver om simulationen kører
        /// </summary>
        public bool IsRunning => _timer.IsEnabled;

        /// <summary>
        /// Antal sekunder der simuleres per tick
        /// </summary>
        public double SecondsPerTick { get; set; } = 1.0;

        /// <summary>
        /// Event der udløses når en bus har lavt batteriniveau
        /// </summary>
        public event Action<Bus> LowBatteryWarning;

        /// <summary>
        /// Opretter en ny simulationsservice
        /// </summary>
        /// <param name="buses">Samlingen af busser der skal simuleres</param>
        /// <param name="weather">Vejrdata der påvirker batteriforbruget</param>
        /// <param name="averageSpeedKmh">Gennemsnitshastighed i km/t</param>
        public SimulationService(ObservableCollection<Bus> buses, Weather weather, double averageSpeedKmh = 20)
        {
            _buses = buses;
            _weather = weather;
            _averageSpeedKmh = averageSpeedKmh;
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (s, e) => SimulateTick();
        }

        /// <summary>
        /// Starter simulationen
        /// </summary>
        public void Start() { _lastTick = DateTime.Now; _timer.Start(); }

        /// <summary>
        /// Stopper simulationen
        /// </summary>
        public void Stop() => _timer.Stop();

        /// <summary>
        /// Simulerer en tidsenhed af busdrift
        /// Håndterer:
        /// - Opdatering af busstatus
        /// - Batteriforbrug baseret på kørt distance
        /// - Opladning af busser
        /// - Advarsler ved lavt batteriniveau
        /// </summary>
        private void SimulateTick()
        {
            var now = DateTime.Now;
            double seconds = SecondsPerTick;
            _lastTick = now;
            double hours = seconds / 3600.0;
            double minutes = seconds / 60.0;
            double consumptionMultiplier = _weather?.ConsumptionMultiplier ?? 1.0;
            
            foreach (var bus in _buses)
            {
                // Opdater simuleret tid først
                if (bus.Status == BusStatus.Inroute || bus.Status == BusStatus.Intercept || bus.Status == BusStatus.Returning || bus.Status == BusStatus.Charging)
                {
                    bus.LastUpdate = bus.LastUpdate.AddSeconds(seconds);
                    bus.TimeInCurrentStatus += minutes;
                }

                // Tjek statusovergange baseret på simuleret tid
                if (bus.Status == BusStatus.Intercept)
                {
                    if (bus.TimeInCurrentStatus >= 30)
                    {
                        bus.Status = BusStatus.Inroute;
                        bus.TimeInCurrentStatus = 0;
                    }
                }
                else if (bus.Status == BusStatus.Inroute && bus.BatteryLevel < 30)
                {
                    var replacementBus = _buses.FirstOrDefault(b => 
                        b.Status == BusStatus.Intercept && 
                        b.Route == bus.Route);
                    
                    if (replacementBus != null && bus.TimeInCurrentStatus >= 30)
                    {
                        bus.Status = BusStatus.Returning;
                        bus.TimeInCurrentStatus = 0;
                    }
                }
                else if (bus.Status == BusStatus.Returning)
                {
                    if (bus.TimeInCurrentStatus >= 30)
                    {
                        bus.Status = BusStatus.Charging;
                        bus.TimeInCurrentStatus = 0;
                    }
                }

                // Opdater batteri for busser i drift
                if (bus.Status == BusStatus.Inroute || bus.Status == BusStatus.Intercept || bus.Status == BusStatus.Returning)
                {
                    double distance = _averageSpeedKmh * hours;
                    double consumptionKWh = distance * bus.Consumption * consumptionMultiplier;
                    double percentUsed = (consumptionKWh / bus.BatteryCapacity) * 100.0;
                    bus.BatteryLevel -= percentUsed;
                    if (bus.BatteryLevel < 0) bus.BatteryLevel = 0;
                    
                    if (bus.BatteryLevel < 30 && !_warnedBuses.Contains(bus))
                    {
                        _warnedBuses.Add(bus);
                        LowBatteryWarning?.Invoke(bus);
                    }
                }
                // Håndter opladning
                else if (bus.Status == BusStatus.Charging)
                {
                    double chargePowerKWh = 150 * hours;
                    double percentCharged = (chargePowerKWh / bus.BatteryCapacity) * 100.0;
                    bus.BatteryLevel += percentCharged;
                    if (bus.BatteryLevel >= 100)
                    {
                        bus.BatteryLevel = 100;
                        bus.Status = BusStatus.Garage;
                        bus.TimeInCurrentStatus = 0;
                    }
                }
            }
        }
    }
} 