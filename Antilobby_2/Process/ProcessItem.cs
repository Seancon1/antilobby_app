using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.name = inName;
            this.timeViewed = 0;
            this.TimeOpen = 0;
        }

        public ProcessItem(string inName, int time)
        {
            this.name = inName;
            this.timeViewed = time;
            this.TimeOpen = 0;

        }

        public ProcessItem get()
        {
            return this;
        }

        public ProcessItem Self(){
            return this;
        }


        public string getName()
        {
            return this.name;
        }
        public int getTime()
        {
            return this.timeViewed;
        }

        public String showNameFormatted()
        {
            return $"{this.name} - {this.timeViewed}";
        }

        /**
         * Logs time, adds time to total and also logs time by the hour
         * */
        public void addTime(int timeQuantity)
        {
            this.timeViewed += timeQuantity;
            this.TimeViewedSpecific[DateTime.Now.Hour, DateTime.Now.Minute] += timeQuantity;
            this.SaveDetailedTime(DateTime.Now.Hour + ":" + DateTime.Now.Minute, timeQuantity);
            //Debug.Print($"Logging: Total time ({TimeViewed}) with {DateTime.Now.Hour}:{DateTime.Now.Minute} at {this.TimeViewedSpecific[DateTime.Now.Hour, DateTime.Now.Minute]}");
        }

        public void logOpenTime()
        {
            this.TimeOpen += 1;
        }

        public int getTimeOpen()
        {
            return this.TimeOpen;
        }

        public int[,] getTimeViewedSpecific()
        {
            return this.TimeViewedSpecific;
        }

        public void SaveDetailedTime(string key, int time)
        {
            if (this.DetailedTime.ContainsKey(key))
            {
                this.DetailedTime[key] += time;
            } else
            {
                this.DetailedTime.Add(key, time);
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
            if(haveBeenSeperated)
            {
                //check if even or odd and split sections based on that, if odd, add 1 to it
                var firstSection = (this.DetailsToSave.Count % 2 == 1) ? (this.DetailsToSave.Count/2) : (this.DetailsToSave.Count +1)/2;
                var secondSection = this.DetailsToSave.Count - firstSection;

                collection.Add(this.DetailsToSave.OrderBy(pair => pair.Key).Take(firstSection).ToDictionary(pair => pair.Key, pair => pair.Value));
                collection.Add(this.DetailsToSave.OrderBy(pair => pair.Key).Skip(firstSection).Take(secondSection).ToDictionary(pair => pair.Key, pair => pair.Value));
                
            }

            return collection;
        }

        public bool SeperateDetailedToSavetime()
        {
            foreach (KeyValuePair<string, int> pair in this.DetailedTime)
            {

                if (this.DetailsToSave.ContainsKey(pair.Key))
                {
                    this.DetailsToSave[pair.Key] = pair.Value;
                }
                else
                {
                    this.DetailsToSave.Add(pair.Key, pair.Value);
                }
            }
            //add Detailed time to dictionary that is pending save
            this.DetailedTime.Clear(); //clear detailed time
            return true;
        }

        public Dictionary<string, int> GetDetailedToSavetime()
        {
            return this.DetailsToSave;
        }

        public void clearDetailedToSaveTime()
        {
            this.DetailsToSave.Clear();
            Debug.Print("Clearing item old save data...");
        }

        public void PrintDebugDetails()
        {
            Debug.WriteLine($"::Item name: {this.getName()}");
            Debug.WriteLine($"::Items not pending save: {this.DetailedTime.Count}");
            Debug.WriteLine($"::Items pending save: {this.DetailsToSave.Count}");
        }

    }
}
