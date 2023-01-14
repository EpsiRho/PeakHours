using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakHoursServer.Classes
{
    public static class Display
    {
        public static List<string> Messages;
        public static void Loop()
        {
            Messages = new List<string>();
            while (true)
            {
                if(Messages.Count() != 0)
                {
                    Console.WriteLine(Messages[0]);
                    Messages.RemoveAt(0);
                    Thread.Sleep(100);
                }
            }
        }
    }
}
