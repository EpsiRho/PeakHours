using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PeakHoursClient.Classes
{
    public class Entry
    {
        public string ID;
        public DateTime Time;
        public DateTime TimeUTC;

        public Entry(string id, DateTime time, DateTime timeUTC)
        {
            ID = id;
            Time = time;
            TimeUTC = timeUTC;
        }
    }
    public class EntryPass
    {
        public Entry entry;
        public Socket sock;
    }
}
