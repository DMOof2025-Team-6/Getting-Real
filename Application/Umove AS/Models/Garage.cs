using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umove_AS.Models
{
    public class Garage
    {
        private double LowBatteryTreshold = 20; //DK grænse for lav batteri
        public string ShiftPlanID; 

        public List<ShiftPlan> shiftplans = new List<ShiftPlan>();

        private List<Bus> busses = new List<Bus>();

        public void AddBus(Bus bus) // TODO: Indfør auto generet ID, når vi ved regler
        {
            if (busses.Exists(b => b.ID == bus.ID)) // Tjekker om bus id eksister i listen.
                throw new ArgumentException("Bus med dette ID findes allerede");
            // TODO: Lav et loop
            busses.Add(bus);

        }


        public void EditBus(string id, double newCapacity, double newKmPerKWh)
        {
            var bus = busses.FirstOrDefault(b => b.ID == id); //Leder efter indput ID'et i listen.
            if (busses == null) throw new ArgumentException("Bus er ikke fundet");
            // TODO: Lav et loop, hvis fejl.
            bus.Update(newCapacity, newKmPerKWh);
        }

        public void RemoveBus(string id)
        {
            var bus = busses.FirstOrDefault(b => b.ID == id);
            if (busses == null)
                throw new ArgumentException("Bus er ikke fundet");

            busses.Remove(bus);
        }

        public List<Bus> GetAllBusses()
        {
            return new List<Bus>(busses);
        }

        public List<Bus> SortByCurrentCharge()//DK
        {
            //The delegate compares the CurrentCharge properties of two Bus objects.
            busses.Sort((bus1, bus2) => bus1.CurrentChargePercent.CompareTo(bus2.CurrentChargePercent));
            return busses;
        }

        public List<Bus> FilterLowBatteryStatus()
        {
            List<Bus> lowBatteryBusses = new List<Bus>();
            foreach (Bus bus in busses)
            {
                if (bus.CurrentChargePercent <= LowBatteryTreshold)
                {
                    lowBatteryBusses.Add(bus);
                }
            }
            return lowBatteryBusses;
        }

        public int GetBatteryStatus(int busID)// -- NOTE: ???New class BusStatus including batterystatus, busname, etc
        {
            Bus bus = busses.FirstOrDefault(b => Convert.ToInt32(b.ID) == busID);
            if (bus != null)
            {
                return bus.CurrentChargePercent;
            }
            else
            {
                throw new ArgumentException($"Bus with ID {busID} not found.");
            }
        }

        public void GetBusStatus()//DK
        {
            foreach (var bus in busses)
            {

            }
        }

        public TimeSpan GetBatteryTimeLeft(double currentChargeKWh, double averageSpeed, double KmPerKWh)//DK
        {
            //afstand der kan køres med current charge
            double distanceKm = currentChargeKWh * KmPerKWh;
            //beregn tid tilbage i timer
            double timeLeftHours = distanceKm / averageSpeed;
            //Konverter timer til TimeSpan
            TimeSpan timeLeft = TimeSpan.FromHours(timeLeftHours);

            return timeLeft;
        }

        public TimeSpan GetChargingTimeLeft(double batteryCapacity, double currentChargeKWh, double chargerSpeed)//DK
        {
            double timeLeftHours = (batteryCapacity - currentChargeKWh) * chargerSpeed;
            TimeSpan timeLeft = TimeSpan.FromHours(timeLeftHours);
            return timeLeft;
        }

        public double PercentageBatteryUsePerHour(TimeSpan operationalTime, double currentChargePercent) //DK
        {
            double batteryPercentUsePerHour = Convert.ToDouble(operationalTime) / (100 - currentChargePercent);
            return batteryPercentUsePerHour;
        }

        public void AddShiftPlan(ShiftPlan shiftPlan) //DK
        {
            if (shiftplans.Exists(s => s.ShiftName == shiftPlan.ShiftName)) // Tjekker om vagtplan id eksister i listen.
                throw new ArgumentException("Vagtplan med dette ID findes allerede");
            // TODO: Lav et loop
            shiftplans.Add(shiftPlan);
        }

        public void RemoveShiftPlan(string shiftPlan) //DK
        {
            var shiftplan = shiftplans.FirstOrDefault(s => s.ShiftName == shiftPlan);
            if (shiftplans == null)
                throw new ArgumentException("Bus er ikke fundet");

            shiftplans.Remove(shiftplan);
        }

        public List<ShiftPlan> GetAllShiftPlans() //DK
        {
            return new List<ShiftPlan>(shiftplans);
        }




        /*public void UpdateBusShiftPlan() //DK Tilknytter Vagtplan til BusID
        {
        }
        public List<Bus> GetAllBusesWithShiftPlans() //DK Viser Vagtplanen til et bestemt BusID
        {

        }*/

    }
}
