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
                if (alert.AlertLimit > this.processList.ReturnTickOf(alert.ProcessName) && alert != null)
                {
                    activeAlerts.Add(alert); //adds the alert to temp list of activeAlerts
                }
            }

            if (activeAlerts.Count < 1) {return null; } else { return activeAlerts;} //return null if no alerts present
                
        }
 
    }
}
