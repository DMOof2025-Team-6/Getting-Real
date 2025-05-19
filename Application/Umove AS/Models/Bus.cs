using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Umove_AS.Services;
using Umove_AS.Type;

namespace Umove_AS.Models
{
    
    public class Bus
    {
        public string ID { get; private set; }
        public string Brand { get; private set; }
        public string Model { get; private set; }
        public string RouteId { get; set; } //Ny 04-05-2025
        public BusStatus Status { get; private set; } //Ny 04-05-2025 (Bruger enum)
        public double BatteryCapacity { get; private set; }
        public double BatteryEfficiencyFactor { get; private set; }
        public double KmPerKWh { get; private set; } //DK(nyt property navn)
        public double KmPerLiter { get; private set; } 
        public double CurrentChargeKWh { get; private set; } 
        public double CurrentChargePercent {get; private set; } 
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
            CurrentChargePercent = 0.8; //samme som nedunder -- RELATIV -- VÆRDI MELLEM 0 - 1
            BatteryEfficiencyFactor = 1;// mulighvis userinput, tester først
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

        public override string ToString()
        {
            //return $"{Brand}, {Model}, {BatteryCapacity}, {CurrentBatteryCharge}%, {BatteryEfficiencyFactor}%";
            return String.Format("{0}, {1}, {2:##%}, {3:#0.##%}", Brand, Model, CurrentChargeKWh, BatteryEfficiencyFactor);
        }

        public TimeSpan CalculateEstimatedTimeBeforeBatteryEmpty(WeatherType currentWeather)
        {
            double weatherModifier = WeatherModifiers.WeatherCondition[currentWeather];

            double currentBatteryCharge = (BatteryCapacity * CurrentChargePercent) * BatteryEfficiencyFactor;
            double currentConsumption = KmPerKWh * weatherModifier;
            double currentConsumptionPercent = 100 / (currentBatteryCharge / currentConsumption);


            double remainingOperationalTime = currentBatteryCharge / currentConsumption;

            TimeSpan timeUntilEmpty = TimeSpan.FromHours(remainingOperationalTime);

            Console.WriteLine($"Bussen bruger på nuværende tidspunkt {Math.Round(currentConsumptionPercent, 2)} % batteri per time. Tid til tomt batteri: {timeUntilEmpty.Hours} timer og {timeUntilEmpty.Minutes} minutter.");

            return TimeSpan.FromHours(remainingOperationalTime);

        }

        public void SelectCurrentWeather()
        {
            Console.WriteLine("Valg nuværende vejr:");
            Console.WriteLine("1. Varm og tør");
            Console.WriteLine("2. Varm og vådt");
            Console.WriteLine("3. Normal og tør");
            Console.WriteLine("4. Normal og vådt");
            Console.WriteLine("5. Koldt og tør");
            Console.WriteLine("6. Koldt og vådt");
            Console.WriteLine("7. Frost og tør");
            Console.WriteLine("8. Frost og vådt");

            Console.WriteLine("Indtast nummeret til korresponderende vejret: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CalculateEstimatedTimeBeforeBatteryEmpty(WeatherType.WarmDry);
                    break;
                case "2":
                    CalculateEstimatedTimeBeforeBatteryEmpty(WeatherType.WarmWet);
                    break;
                case "3":
                    CalculateEstimatedTimeBeforeBatteryEmpty(WeatherType.AverageDry);
                    break;
                case "4":
                    CalculateEstimatedTimeBeforeBatteryEmpty(WeatherType.AverageWet);
                    break;
                case "5":
                    CalculateEstimatedTimeBeforeBatteryEmpty(WeatherType.ColdDry);
                    break;
                case "6":
                    CalculateEstimatedTimeBeforeBatteryEmpty(WeatherType.ColdWet);
                    break;
                case "7":
                    CalculateEstimatedTimeBeforeBatteryEmpty(WeatherType.FreezeDry);
                    break;
                case "8":
                    CalculateEstimatedTimeBeforeBatteryEmpty(WeatherType.FreezeWet);
                    break;
                default:
                    Console.WriteLine("Ugyldig valg. Vælg venligst en gyldig vejrtilstand.");
                    break;

            }
        }



    }
}
