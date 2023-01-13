using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakHoursServer.Classes
{
    public class Entry
    {
        public string ID;
        public DateTime Time;

        public Entry(string id, DateTime time)
        {
            ID = id;
            Time = time;
        }
    }
}
