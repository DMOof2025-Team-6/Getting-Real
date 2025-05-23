using System;

namespace UMOVEWPF.Models
{
    /// <summary>
    /// Enum der definerer de forskellige statusser en bus kan have
    /// </summary>
    public enum BusStatus
    {
        /// <summary>
        /// Bussen er i rute og kører normalt
        /// </summary>
        Inroute,
        
        /// <summary>
        /// Bussen er på vej for at erstatte en anden bus
        /// </summary>
        Intercept, // Status for, hvis den er på vej til at erstatte en anden bus.
        
        /// <summary>
        /// Bussen er på vej tilbage til garagen
        /// </summary>
        Returning, // Bus der er blevet aflyst og på vej tilbage for at charge.
        
        /// <summary>
        /// Bussen er ledig og kan tildeles en ny rute
        /// </summary>
        Free, // Måske fjernes
        
        /// <summary>
        /// Bussen er i garagen
        /// </summary>
        Garage,
        
        /// <summary>
        /// Bussen er i opladning
        /// </summary>
        Charging,
        
        /// <summary>
        /// Bussen er i reparation
        /// </summary>
        Repair
    }
} 