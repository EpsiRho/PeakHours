using Newtonsoft.Json;
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
        public static List<Entry> entries;
        public static bool Init()
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

            LoadLocalEntries();

            if(entries.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public static void LoadLocalEntries()
        {
            entries = new List<Entry>();

            if (File.Exists("LocalEntries.json"))
            {
                string text = File.ReadAllText("LocalEntries.json");
                entries = JsonConvert.DeserializeObject<List<Entry>>(text);
            }
        }

        public static void SaveLocalEntry(Entry e)
        {
            entries.Add(e);

            string text = JsonConvert.SerializeObject(entries);
            File.WriteAllText("LocalEntries.json", text);
        }
    }
}
