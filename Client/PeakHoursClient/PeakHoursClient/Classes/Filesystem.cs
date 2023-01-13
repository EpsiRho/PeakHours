using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakHoursClient.Classes
{
    public static class Filesystem
    {
        public static string ID;
        public static void Init()
        {
            ID = "";
            if (!File.Exists("LocalState.stor"))
            {
                Random rand = new Random();
                for (int i = 0; i < 4; i++)
                {
                    ID += rand.Next(0, 9);
                }

                File.WriteAllText("LocalState.stor", ID);
            }
            else
            {
                ID = File.ReadAllText("LocalState.stor");
            }
        }
    }
}
