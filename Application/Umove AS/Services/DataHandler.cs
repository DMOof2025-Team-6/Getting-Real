using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umove_AS.Models;
using System.IO;

namespace Umove_AS.Services
{
    public class DataHandler
    {
        public string FilePath { get; set; }

       public DataHandler(string filePath)
        {
            FilePath = filePath;
        }

        public void SaveAllToFile(List<Garage> busses)
        {
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                foreach (var bus in busses)
                {
                    foreach (var shiftplan in bus.shiftplans)
                    {
                        sw.WriteLine(shiftplan.ToString());
                    }
                }
            }
        }

        public List<ShiftPlan>LoadShiftPlan()
        {
            List<ShiftPlan> shiftplans = new List<ShiftPlan>();

            using (StreamReader sr = new StreamReader(FilePath))
            {
                string line;
                while ((line = sr.ReadLine()) !=null)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        shiftplans.Add(ShiftPlan.FromString(line));
                    }
                }
            }



            return shiftplans;

        }
    }
}
