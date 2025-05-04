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
            var kmPerKWh = double.Parse(Console.ReadLine());
            Console.Write("Vagtplan: "); //DK
            var shiftname = Console.ReadLine(); //DK

            try
            {
                var shiftPlan = new ShiftPlan(shiftname);// DK : Create a ShiftPlan object

                garage.AddBus(new Bus(id, capacity, kmPerKWh, shiftPlan));// DK : added shiftPlan to constructor
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
                Console.WriteLine($"ID: {bus.ID}, Kapacitet: {bus.BatteryCapacity}, Forbrug: {bus.KmPerKWh}");
                
            }
            Console.ReadKey ();

        }

        public void CreateShiftPlan() //DK
        {
            Console.Write("Vagtplan: ");
            var shiftname = Console.ReadLine();

            try
            {
                garage.AddShiftPlan(new ShiftPlan(shiftname));// DK : added shiftPlan to constructor
                Console.WriteLine("VagtPlan Oprret");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        public void DeleteShiftPlan()
        {
            Console.Write("Vagtplan: ");
            var id = Console.ReadLine();

            try
            {
                garage.RemoveShiftPlan(id);
                Console.WriteLine("Vagtplan Slettet");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        public void ShowShiftPlans()
        {
            var shiftplans = garage.GetAllShiftPlans();
            foreach (var shiftplan in shiftplans)
            {
                Console.WriteLine($"ID: {shiftplan.ShiftName}");

            }
            Console.ReadKey();

        }


    }
}
