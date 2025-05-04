using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umove_AS.Models
{
    public class ShiftPlan
    {
        public string ShiftName { get; set; }

        public ShiftPlan(string shiftName)
        {
            ShiftName = shiftName;
        }
    }
}
