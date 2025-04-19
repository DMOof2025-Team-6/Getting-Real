﻿using Umove_AS.UI;
using System.Collections.Generic;

namespace Umove_AS
{
    class Program
    {
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
                new ("Chauffør",        () => menu.Push(BuildDriverMenu())),
                new ("Driftmedarbejder",() => menu.Push(BuildOperationStaffMenu())),
                new ("IT-Medarbejder",  () => menu.Push(BuildITWorkerMenu())),

                
            }); // ); afslutter metode & constructor kaldet. Alt inde i new, er parametere.

        private static MenuPage BuildDriverMenu() => new(
            "Chaufør",
            new List<MenuItem>
            {
                new ("Indtast batteriprocent for bussen", PromptBatteryPercentage),
                new ("Fejlrapport: hurtig opladning",      PromptFastChargeError),
                
            });

        private static MenuPage BuildOperationStaffMenu() => new(
            "Driftmedarbejder",
            new List<MenuItem>
            {
                new ("Administrer stam bus‑data",   PromptBusMasterData),
                new ("Overvåg batteristatus",       PromptBatteryMonitor),
                
            });

        private static MenuPage BuildITWorkerMenu() => new(
            "Driftmedarbejder",
            new List<MenuItem>
            {
                new ("Rediger Chauføre ",    PromtDummy),
                new ("Rediger Busser",       PromtDummy),

            });


        // --- DUMMY PROMPTS --- Er eksempler, og metoderne skal være Model mappen.
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
        private static void PromtDummy()
        {
            Console.WriteLine("Det her er en dummy");
            Console.ReadKey();
        }
    }
}
