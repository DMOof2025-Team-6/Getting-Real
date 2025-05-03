using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umove_AS.Services;
using Umove_AS.Models;

namespace Umove_AS.UI
{
    public class UIGarage
    {
        private GarageHandler garage = new GarageHandler();

        public void CreateBus()
        {
            Console.Write("ID: ");
            var id = Console.ReadLine();
            Console.Write("BatteriKapacitet: ");
            var capacity = double.Parse(Console.ReadLine());
            Console.Write("Forbrug: ");
            var usage = double.Parse(Console.ReadLine());

            try
            {
                garage.AddBus(new Bus(id, capacity, usage));
                Console.WriteLine("Bus Oprret");
                Console.ReadKey();
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        public void EditBus()
        {
            Console.Write("ID: ");
            var id = Console.ReadLine();
            Console.Write("Ny batterikapacitet: ");
            var capacity = double.Parse(Console.ReadLine());
            Console.Write("Nyt forbrug: ");
            var usage = double.Parse(Console.ReadLine());

            try
            {
                garage.EditBus(id, capacity, usage);
                Console.WriteLine("Bus opdateret");
                Console.ReadKey();
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
            
        }

        public void DeleteBus()
        {
            Console.Write("ID: ");
            var id = Console.ReadLine();

            try
            {
                garage.RemoveBus(id);
                Console.WriteLine("Bus Slette");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        public void ShowBusses()
        {
            var busses = garage.GetAllBusses();
            foreach (var bus in busses)
            {
                Console.WriteLine($"ID: {bus.ID}, Kapacitet: {bus.BatteryCapacity}, Forbrug: {bus.Usage}");
                
            }
            Console.ReadKey ();

        }
    }
}
