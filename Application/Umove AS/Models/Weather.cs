using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Alle stamdata vedr vejr indeholdes her. 

namespace Umove_AS.Data
{
    internal class Weather
    {
        enum Season
        {
            Winter,
            Spring,
            Summer,
            Autumn
        }
        private double _temperature; //Temperatur i grader Celsius
        private bool _IsRaining; //True hvis det regner
    }
}
