using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umove_AS.Type;

namespace Umove_AS.Models
{
    
    public class Bus
    {
        public string ID { get; private set; }
        public string RouteId { get; set; } //Ny 04-05-2025
        public BusStatus Status { get; private set; } //Ny 04-05-2025 (Bruger enum)
        public double BatteryCapacity { get; private set; }
        public double KmPerKWh { get; private set; } //DK(nyt property navn)
        public double KmPerLiter { get; private set; } 
        public double CurrentChargeKWh { get; private set; } 
        public int CurrentChargePercent {get; private set; } 
        public double ChargerSpeed { get; private set; } 
        public TimeSpan OperationalTime { get; private set; }
        public string Location { get; private set; } 
        public double AverageSpeed { get; private set; }
        public ShiftPlan ShiftPlan { get; private set; }

        


        public Bus(string id, double batterycapacity, double kmPerKWh, ShiftPlan shiftPlan)
        {
            ID = id;
            Status = BusStatus.Garage;
            BatteryCapacity = batterycapacity;
            KmPerKWh = kmPerKWh;
            ShiftPlan = shiftPlan;
        }

        public void Update(double newCapacity, double newKmPerKWh)
        {
            BatteryCapacity = newCapacity;
            KmPerKWh = newKmPerKWh;
        }

        public string GetBusStatus()
        {
            return $"Bus {ID} - Lokation: {Location} - {Status} - {CurrentChargePercent}%"; //Ny 04-05-2025 //TODO flere variabler, der giver mening.
        }

        public void UpdateStatus(BusStatus newStatus)
        {
            Status = newStatus;
        }



    }
}
