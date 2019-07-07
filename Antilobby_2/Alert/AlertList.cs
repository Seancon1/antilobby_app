using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antilobby_2.Alert
{
    class AlertList
    {
        private List<Alert> list;
        private AntiLobby_2.ProcessList processList;
            
        public AlertList(AntiLobby_2.ProcessList processList)
        {
            this.list = new List<Alert>();
            this.processList = processList; //must be able to access the current processList
        }

        /*
         * Returns (a list) of all alerts inside the this.list with a tick count that exceeds their limit'
         * */
        public List<Alert> ActiveAlerts()
        {

            List<Alert> activeAlerts = new List<Alert>();

            foreach (Alert alert in this.list)
            {
                if (alert.AlertLimit < this.processList.ReturnTickOf(alert.ProcessName) && alert != null)
                {
                    activeAlerts.Add(alert); //adds the alert to temp list of activeAlerts
                }
            }

            if (activeAlerts.Count < 1) {return null; } else { return activeAlerts;} //return null if no alerts present
                
        }

        public List<Alert> PassiveAlerts()
        {
            List<Alert> passiveAlerts = new List<Alert>();

            foreach (Alert alert in this.list)
            {
                if (alert.AlertLimit > this.processList.ReturnTickOf(alert.ProcessName) && alert != null)
                {
                    passiveAlerts.Add(alert); //adds the alert to temp list of passiveAlerts
                }
            }

            if (passiveAlerts.Count < 1) { return null; } else { return passiveAlerts; } //return null if no alerts present


        }

        /*
        public Alert GetAlertTime(string name)
        {
            if()
        }
        */

        public bool addNewAlert(string processName, int alertTime)
        {
            //Add a new alert to the list
            try
            {
            this.list.Add(new Alert(processName, alertTime));
            } catch (Exception ee)
            {
                return false;
            }
            return true;
        }

        public bool removeAlert(Alert alert)
        {
            //Add a new alert to the list
            try
            {
                this.list.Remove(alert);
            }
            catch (Exception ee)
            {
                return false;
            }
            return true;
        }

        public int fetchAlertTime(string processName)
        {
            foreach(Alert alert in this.list) {
                if(alert.ProcessName == processName)
                {
                    return alert.AlertLimit; //return if processName equals the same that is being searched
                }
            }

            return -1;
        }

        public bool isEmpty()
        {
            if(list.Count() > 0)
            {
                return false;
            } else
            {
                return true;
            }
        }

    }
}
