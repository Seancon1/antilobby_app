using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antilobby_2.Alert
{
    /*
     * Alerts class
     * Creates an object with information for an alert.
     * 
     * */
    class Alert
    {
        private string processName = null;
        private int alertLimit = 0;

        //Default constructor, must have both, default limit is 250 ticks
        public Alert(string inProcessName, int inAlertLimit = 250)
        {
            this.processName = inProcessName;
            this.alertLimit = inAlertLimit;
        }

        public int AlertLimit { get => alertLimit; set => alertLimit = value; }
        public string ProcessName { get => processName; set => processName = value; }
    }
}
