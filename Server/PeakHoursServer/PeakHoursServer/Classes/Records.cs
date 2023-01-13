using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PeakHoursServer.Classes
{
    public static class Records
    {
        private static List<Entry> entries;
        public static void SaveEntry(Entry e)
        {
            // Add to stack
            entries.Add(e);

            // Save stack to file
            string text = JsonConvert.SerializeObject(entries);
            File.WriteAllText(".\\Entries.json", text);
        }
        public static void LoadEntries() 
        {
            try
            {
                // Init stack
                entries = new List<Entry>();

                // Load file in and convert
                string text = File.ReadAllText(".\\Entries.json");
                entries = JsonConvert.DeserializeObject<List<Entry>>(text);

                Display.Messages.Add($"[!] Previous Entries Loaded");
            }
            catch(Exception e) 
            {
                Display.Messages.Add($"[!] Couldn't load previous entries, likely because there are none.");
            }
        }
    }
}
