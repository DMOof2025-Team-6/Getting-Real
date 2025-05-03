using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umove_AS.Models;

namespace Umove_AS.Services
{
    public class GarageHandler
    {
        private List<Bus> busses = new List<Bus>();

        public void AddBus(Bus bus) // TODO: Indfør auto generet ID, når vi ved regler
        {
            if (busses.Exists(b => b.ID == bus.ID)) // Tjekker om bus id eksister i listen.
                throw new ArgumentException("Bus med dette ID findes allerede");
            // TODO: Lav et loop
            busses.Add(bus);
            
        }

        public void EditBus(string id, double newCapacity, double newUsage)
        {
            var bus = busses.FirstOrDefault(b => b.ID == id); //Leder efter indput ID'et i listen.
            if (busses == null) throw new ArgumentException("Bus er ikke fundet");
            // TODO: Lav et loop, hvis fejl.
            bus.Update(newCapacity, newUsage);
        }

        public void RemoveBus(string id)
        {
            var bus = busses.FirstOrDefault(b =>b.ID == id);
            if (busses == null) 
                throw new ArgumentException("Bus er ikke fundet");

            busses.Remove(bus);
        }

        public List<Bus> GetAllBusses()
        {
            return new List<Bus>(busses);
        }
    }
}
