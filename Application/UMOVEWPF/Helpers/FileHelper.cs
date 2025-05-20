using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UMOVEWPF.Models;

namespace UMOVEWPF.Helpers
{
    public static class FileHelper
    {
        private static readonly string BusesFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "buses.txt");
        private static readonly string RoutesFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "routes.txt");
        private static readonly System.Threading.SemaphoreSlim _busesLock = new System.Threading.SemaphoreSlim(1, 1);

        public static async Task SaveBusesAsync(IEnumerable<Bus> buses)
        {
            await _busesLock.WaitAsync();
            try
            {
                using (var sw = new StreamWriter(BusesFile, false))
                {
                    foreach (var bus in buses)
                    {
                        await sw.WriteLineAsync($"{bus.BusId};{bus.Year};{bus.Route};{bus.BatteryLevel};{bus.Status};{bus.LastUpdate:O};{bus.Model}");
                    }
                }
            }
            finally { _busesLock.Release(); }
        }

        public static async Task<List<Bus>> LoadBusesAsync()
        {
            await _busesLock.WaitAsync();
            try
            {
                var buses = new List<Bus>();
                if (!File.Exists(BusesFile))
                    return buses;

                using (var sr = new StreamReader(BusesFile))
                {
                    string line;
                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        var parts = line.Split(';');
                        if (parts.Length >= 7)
                        {
                            buses.Add(new Bus
                            {
                                BusId = parts[0],
                                Year = parts[1],
                                Route = Enum.TryParse<RouteName>(parts[2], out var route) ? route : RouteName.None,
                                BatteryLevel = double.TryParse(parts[3], out var lvl) ? lvl : 0,
                                Status = Enum.TryParse<BusStatus>(parts[4], out var stat) ? stat : BusStatus.Garage,
                                LastUpdate = DateTime.TryParse(parts[5], out var dt) ? dt : DateTime.Now,
                                Model = Enum.TryParse<BusModel>(parts[6], out var model) ? model : BusModel.MBeCitaro
                            });
                        }
                    }
                }
                return buses;
            }
            finally { _busesLock.Release(); }
        }

        public static async Task SaveRoutesAsync(IEnumerable<Route> routes)
        {
            using (var sw = new StreamWriter(RoutesFile, false))
            {
                foreach (var route in routes)
                {
                    await sw.WriteLineAsync($"{route.Name};{route.Description};{route.Distance};{route.EstimatedTime}");
                }
            }
        }

        public static async Task<List<Route>> LoadRoutesAsync()
        {
            var routes = new List<Route>();
            if (!File.Exists(RoutesFile))
                return routes;

            using (var sr = new StreamReader(RoutesFile))
            {
                string line;
                while ((line = await sr.ReadLineAsync()) != null)
                {
                    var parts = line.Split(';');
                    if (parts.Length >= 4)
                    {
                        routes.Add(new Route
                        {
                            Name = Enum.TryParse<RouteName>(parts[0], out var name) ? name : RouteName.None,
                            Description = parts[1],
                            Distance = double.TryParse(parts[2], out var dist) ? dist : 0,
                            EstimatedTime = int.TryParse(parts[3], out var time) ? time : 0
                        });
                    }
                }
            }
            return routes;
        }
    }
} 