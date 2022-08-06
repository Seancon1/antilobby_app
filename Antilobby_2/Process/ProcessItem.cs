using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AntiLobby_2
{
    public class ProcessItem
    {

        private string name;
        private int timeViewed;
        private int[,] TimeViewedSpecific = new int[25, 61];
        private Dictionary<string, int> DetailedTime = new Dictionary<string, int>();
        private Dictionary<string, int> DetailsToSave = new Dictionary<string, int>();
        public int TimeOpen = 0;

        /* 
         * Set up so JSON can serialize the object
         * */
        public string Name { get => name; set => name = value; }
        public int TimeViewed { get => timeViewed; set => timeViewed = value; }
        //public int[,] TimeViewedSpecific { get => TimeViewedSpecific; set => TimeViewedSpecific = value; }
        //public int TimeOpen { get => TimeOpen; set => TimeOpen = value; }
        /* */

        public ProcessItem()
        {

        }

        public ProcessItem(string inName)
        {
            name = inName;
            timeViewed = 0;
            TimeOpen = 0;
        }

        public ProcessItem(string inName, int time)
        {
            name = inName;
            timeViewed = time;
            TimeOpen = 0;

        }

        public ProcessItem get()
        {
            return this;
        }

        public ProcessItem Self()
        {
            return this;
        }


        public string getName()
        {
            return name;
        }
        public int getTime()
        {
            return timeViewed;
        }

        public String showNameFormatted()
        {
            return $"{name} - {timeViewed}";
        }

        /**
         * Logs time, adds time to total and also logs time by the hour
         * */
        public void addTime(int timeQuantity)
        {
            timeViewed += timeQuantity;
            TimeViewedSpecific[DateTime.Now.Hour, DateTime.Now.Minute] += timeQuantity;
            SaveDetailedTime(DateTime.Now.Hour + ":" + DateTime.Now.Minute, timeQuantity);
            //Debug.Print($"Logging: Total time ({TimeViewed}) with {DateTime.Now.Hour}:{DateTime.Now.Minute} at {this.TimeViewedSpecific[DateTime.Now.Hour, DateTime.Now.Minute]}");
        }

        public void logOpenTime()
        {
            TimeOpen += 1;
        }

        public int getTimeOpen()
        {
            return TimeOpen;
        }

        public int[,] getTimeViewedSpecific()
        {
            return TimeViewedSpecific;
        }

        public void SaveDetailedTime(string key, int time)
        {
            if (DetailedTime.ContainsKey(key))
            {
                DetailedTime[key] += time;
            }
            else
            {
                DetailedTime.Add(key, time);
            }

        }

        /**
         * Not sure if this convoluted, perhaps I can simply this logic even further?
         * Call DoSavePrep so do all functions needed before saving functions
         * 
         * */
        public List<Dictionary<string, int>> ReturnPreparedList()
        {
            List<Dictionary<string, int>> collection = new List<Dictionary<string, int>>();
            var haveBeenSeperated = SeperateDetailedToSavetime();
            if (haveBeenSeperated)
            {
                //check if even or odd and split sections based on that, if odd, add 1 to it
                var firstSection = (DetailsToSave.Count % 2 == 1) ? (DetailsToSave.Count / 2) : (DetailsToSave.Count + 1) / 2;
                var secondSection = DetailsToSave.Count - firstSection;

                collection.Add(DetailsToSave.OrderBy(pair => pair.Key).Take(firstSection).ToDictionary(pair => pair.Key, pair => pair.Value));
                collection.Add(DetailsToSave.OrderBy(pair => pair.Key).Skip(firstSection).Take(secondSection).ToDictionary(pair => pair.Key, pair => pair.Value));

            }

            return collection;
        }

        public bool SeperateDetailedToSavetime()
        {
            foreach (KeyValuePair<string, int> pair in DetailedTime)
            {

                if (DetailsToSave.ContainsKey(pair.Key))
                {
                    DetailsToSave[pair.Key] = pair.Value;
                }
                else
                {
                    DetailsToSave.Add(pair.Key, pair.Value);
                }
            }
            //add Detailed time to dictionary that is pending save
            DetailedTime.Clear(); //clear detailed time
            return true;
        }

        public Dictionary<string, int> GetDetailedToSavetime()
        {
            return DetailsToSave;
        }

        public void clearDetailedToSaveTime()
        {
            DetailsToSave.Clear();
            Debug.Print("Clearing item old save data...");
        }

        public void PrintDebugDetails()
        {
            Debug.WriteLine($"::Item name: {getName()}");
            Debug.WriteLine($"::Items not pending save: {DetailedTime.Count}");
            Debug.WriteLine($"::Items pending save: {DetailsToSave.Count}");
        }

    }
}
