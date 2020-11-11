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
            Debug.Print($"Logging: Total time ({TimeViewed}) with {DateTime.Now.Hour}:{DateTime.Now.Minute} at {this.TimeViewedSpecific[DateTime.Now.Hour, DateTime.Now.Minute]}");
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
    }
}
