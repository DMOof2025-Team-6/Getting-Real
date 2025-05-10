using Umove_AS.UI;
using System.Collections.Generic;
using Umove_AS.Services;
using Umove_AS.Type;

namespace Umove_AS
{
    class Program
    {
        private static readonly BusService busService = new();
        private static readonly ShiftPlanService shiftPlanService = new();
        private static readonly GarageService garageService = new(busService, shiftPlanService);
        private static readonly GarageUI garageUI = new(garageService);
        


        /// <summary>
        /// Opretter 2 objecter, og er 2 constructore involveret. (MenuManger & standardIConsole.)
        /// Det betyder at RealConsole som er i IConsole, indlæser indput og gemmer dem.
        /// Menumanger, bruger indputtet og bruger det videre til output.
        /// Static for at man kun behøver at oprette det en gang og det bruges,
        /// af alle funktioner.
        /// Readonly gør at man ikke kan overskrive den.
        /// </summary>
        private static readonly MenuManager menu = new(new RealConsole());

        /// <summary> /// Kalder bevægelse funktion fra MenuManager, samt hovedmenu /// </summary>
        static void Main()
        {
            menu.Navigate(BuildMainMenu()); //Kalder bevægelse funktion fra MenuManager, samt hovedmenu.
        }

        // Laver vores menuer.

        // Hovedmenu, 
        private static MenuPage BuildMainMenu() => new(
            "UMOVE A/S",
            new List<MenuItem>
            {
                new ("Chauffør",            () => menu.Push(BuildDriverMenu())),
                new ("Driftmedarbejder",    () => menu.Push(BuildOperationStaffMenu())),
                new ("IT-Medarbejder",      () => menu.Push(BuildITWorkerMenu())),

                
            }); // ); afslutter metode & constructor kaldet. Alt inde i new, er parametere.

        //Alt med Chaufør menu
        private static MenuPage BuildDriverMenu() => new(
            "Chaufør",
            new List<MenuItem>
            {
                new ("Indtast batteriprocent for bussen", PromptBatteryPercentage),
                new ("Fejlrapport: hurtig opladning",      PromptFastChargeError),
                
            });

        //Alt med DriftMedarbejder
        private static MenuPage BuildOperationStaffMenu() => new(
            "Driftmedarbejder",
            new List<MenuItem>
            {
                new ("Administrer stam bus‑data",   () => menu.Push(AdminstrateBusData())),
                new ("Overvåg batteristatus",       () => menu.Push(ShowBatteriStatus())),
                new ("Administrer vagtplaner",      () => menu.Push(AdministrateShiftPlans())),

            });

        private static MenuPage AdminstrateBusData() => new(
            "Driftmedarbejder",
            new List<MenuItem>
            {
                new ("Opret bus",           () => garageUI.CreateBus()),
                new ("Rediger bus",         () => garageUI.EditBus()),
                new ("Opdater bus status",  () => garageUI.UpdateBusStatus()),
                new ("Slet bus",            () => garageUI.DeleteBus()),
                new ("Vis busser",          () => garageUI.ShowBusses()),
            });

        private static MenuPage ShowBatteriStatus() => new(
           "Driftmedarbejder",
           new List<MenuItem>
           {
                new ("Vis busser", () => garageUI.ShowBusses()),
           });

        private static MenuPage AdministrateShiftPlans() => new(
           "Driftmedarbejder",
           new List<MenuItem>
           {
                new ("Opret vagtplan", () => garageUI.CreateShiftPlan()),
                new ("Slet vagtplan", () => garageUI.DeleteShiftPlan()),
                new ("Vis vagtplaner", () => garageUI.ShowShiftPlans()),
           });

        private static MenuPage BuildITWorkerMenu() => new(
            "Driftmedarbejder",
            new List<MenuItem>
            {
                new ("Rediger Chauføre ",    PromptDummy),
                new ("Rediger Busser",       PromptDummy),

            });

       


        // Dummy Prompts.. Er eksempler, og metoderne skal være Model mappen.
        private static void PromptBatteryPercentage()
        {
            Console.Write("Angiv batteriprocent: ");
            var _ = Console.ReadLine();
            Console.WriteLine("Gemmer værdien...");
            Console.ReadKey();
        }

        private static void PromptFastChargeError()
        {
            Console.WriteLine("Fejlrapport registreret!");
            Console.ReadKey();
        }

        private static void PromptBusMasterData()
        {
            Console.WriteLine("Stamdata‑redigering (dummy).");
            Console.ReadKey();
        }

        private static void PromptBatteryMonitor()
        {
            Console.WriteLine("Viser batteriovervågning (dummy).");
            Console.ReadKey();
        }
        private static void PromptDummy()
        {
            Console.WriteLine("Det her er en dummy");
            Console.ReadKey();
        }
    }
}
