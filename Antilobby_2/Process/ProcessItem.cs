using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiLobby_2
{
    public class ProcessItem
    {

        private string name;
        private int timeViewed;

        public ProcessItem(string inName)
        {
            this.name = inName;
            this.timeViewed = 0;
        }

        public ProcessItem get()
        {
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
            return "" + this.name + " - " + this.timeViewed;
        }

        public void addTime(int timeQuantity)
        {
            this.timeViewed += timeQuantity;
        }

   
    }
}
