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
        private int currentCount = 0;
        private string alertAction = "none";


        //Default constructor, must have both, default limit is 250 ticks
        public Alert(string inProcessName, int inAlertLimit = 250)
        {
            this.processName = inProcessName;
            this.alertLimit = inAlertLimit;
            this.currentCount = 0;
            this.AlertAction = "none";
        }

        public Alert(string inProcessName, int inAlertLimit = 250, string AlertAction = "none")
        {
            this.processName = inProcessName;
            this.alertLimit = inAlertLimit;
            this.currentCount = 0;
            this.AlertAction = AlertAction;
        }

        public int AlertLimit { get => alertLimit; set => alertLimit = value; }
        public int CurrentCount { get => currentCount; set => currentCount = value; }
        public string ProcessName { get => processName; set => processName = value; }
        public string AlertAction { get => alertAction; set => alertAction = value; }

        public void addTick()
        {
            currentCount++;
        }

        public void resetTick()
        {
            currentCount = 0;
        }
    }
}
