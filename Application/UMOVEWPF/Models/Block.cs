using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace UMOVEWPF.Models
{
    /// <summary>
    /// Repræsenterer et vognløb i systemet
    /// En vognløb er en tidsperiode hvor en bus er tildelt til en specifik rute
    /// </summary>
    internal class Block
    {
        //Grundvariabler er indkapslede og beskyttede. 

        private int _blockId { get; set; } //Vognløb id.
        private DateTime _blockStart; //Starttidspunkt for vognløbet.
        private DateTime _blockEnd; //Sluttidspunkt for vognløbet.
        private double _blockTimeLeftInService; //Beregner hvor lang tid der er tilbage for bussen i et tilsat vognløb. (lav DateTime.Now - blockEnd, så har vi tid tilbage i rute)
        private double _blockLength; //Længden på et vognløb beregnes fra starttid og sluttid. Att Martin, hører til i viewmodel.(BlockStart - BlockEnd) * 60 = længde i minutter.Kan bruges til at udregne om en bus kan klare vognløb. (Kapacitet/forbrug/tid)
        private double _blockPause; //Inaktive perioder i aktive vognløb hvor bussen ikke er i drift og derfor ikke bruger strøm.  (kan være 0, hvis der ikke er pause) Att Martin, hører til i viewmodel. (BlockLength - BlockTimeLeftInService) * 60 = pause i minutter. Kan bruges til at udregne om en bus kan klare vognløb. (Kapacitet/forbrug/tid)
        private string _route; //Buslinje som det respektive vognløb indeholder. OBS!! Tænker liste i ViewModel, hvis ikke tid, så en enum. Eventuelt undlades, ikke kritisk for funktion!!

        //Modificerbare variabler påbegyndes nu - input valideres i set metoderne.

        /// <summary>
        /// Vognløbets unikke identifikationsnummer
        /// </summary>
        public int BlockId
        {
            get { return _blockId; }
            set
            {
                if (value <= 0) 
                    throw new ArgumentException("Et vognløb skal have en værdi som er større end 0."); //Martin - husk evt try-catch i metoderne som skal hente herfra, så vi ikke får runtime Crash. 

                _blockId = value;
            }
        }

        /// <summary>
        /// Starttidspunkt for vognløbet
        /// </summary>
        public DateTime BlockStart
        {
            get { return _blockStart; }
            set
            {
                _blockStart = value;
            }
        }

        /// <summary>
        /// Sluttidspunkt for vognløbet
        /// </summary>
        public DateTime BlockEnd
        {
            get { return _blockEnd; }
            set
            {
                _blockEnd = value;
            }
        }

        /// <summary>
        /// Tid tilbage i drift for vognløbet
        /// </summary>
        public double BlockTimeLeftInService //Udkommenter hele kodestykke eller slet hvis undlades
        {
            get { return _blockTimeLeftInService; }
            set
            {
                _blockTimeLeftInService = value;
            }
        }

        /// <summary>
        /// Inaktive perioder i aktive vognløb hvor bussen ikke er i drift og derfor ikke bruger strøm
        /// </summary>
        public double BlockPause //Inaktive perioder i aktive vognløb hvor bussen ikke er i drift og derfor ikke bruger strøm. 
        {
            get { return _blockPause; }
            set
            {
                _blockPause = value;
            }
        }

        /// <summary>
        /// Buslinjen som vognløbet kører på
        /// </summary>
        public string Route //String , ikke int, fordi rute kan indeholde bogstaver "1A", "200S", "93N" osv. 
        {
            get { return _route; } 
            set
            {
                if (string.IsNullOrWhiteSpace(value)) //Der må ikke indtastes en tom værdi.
                    throw new ArgumentException("Vognløbet skal have en buslinje."); //Martin - husk evt try-catch i metoderne som skal hente herfra, så vi ikke får runtime Crash. 
                _route = value;
            }
        }
    }
}
