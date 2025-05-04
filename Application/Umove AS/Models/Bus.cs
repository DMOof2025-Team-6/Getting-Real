using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umove_AS.Models
{
    enum BusStatus
    {
        Inroute,
        Free,
        Garage,
        Charging,
        Repair
    }
    public class Bus
    {
        public string ID { get; private set; }
        public double BatteryCapacity { get; private set; }
        public double KmPerKWh { get; private set; } //DK(nyt property navn)
        public double KmPerLiter { get; private set; } //DK
        public double CurrentChargeKWh { get; private set; } //DK
        public int CurrentChargePercent {get; private set; } //DK
        public double ChargerSpeed { get; private set; } //DK
        public TimeSpan OperationalTime { get; private set; }
        public string Location { get; private set; } //DK
        public double AverageSpeed { get; private set; }//DK
        public ShiftPlan ShiftPlan { get; private set; }
        


        public Bus(string id, double batterycapacity, double kmPerKWh, ShiftPlan shiftPlan)
        {
            ID = id;
            BatteryCapacity = batterycapacity;
            KmPerKWh = kmPerKWh;
            ShiftPlan = shiftPlan;
        }

        public void Update(double newCapacity, double newKmPerKWh)
        {
            BatteryCapacity = newCapacity;
            KmPerKWh = newKmPerKWh;
        }



    }
}
