using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Antilobby_2.Alert
{
    class AlertList
    {

        private AlertPlay alertplay;
        private List<Alert> list;
        private List<Alert> disposeList;
        private AntiLobby_2.ProcessList processList;
        private AlertAction alertAction = new AlertAction();

            
        public AlertList(AntiLobby_2.ProcessList processList)
        {
            this.list = new List<Alert>();
            this.disposeList = new List<Alert>();
            this.processList = processList; //must be able to access the current processList
        }

        /**
         * String exemptProcess - the process that will be exempt when incrementing all process ticks
         * */
        public void incrementAllTicks(String exemptProcess)
        {
            foreach (Alert alert in this.list)
            {
                /*
                 * exempt process is the current active process, so don't increment the AFK timer.
                 * This AFK alert is for continuous seconds NOT ACTIVELY on, so a reset is required
                */ 
                if(alert.AlertAction == "close")
                {
                    alert.addTick();
                } else
                {
                    if (alert.ProcessName == exemptProcess) { alert.resetTick(); } else
                    {
                        alert.addTick();
                    }
                }
                
            }

        }

        /*
         * Returns (a list) of all alerts inside the this.list with a tick count that exceeds their limit'
         * */
        public List<Alert> ActiveAlerts()
        {
            AlertPlay alertSound = new AlertPlay();
            List<Alert> activeAlerts = new List<Alert>();

            foreach (Alert alert in this.list)
            {
                if (alert.AlertLimit < fetchTick(alert.ProcessName) && alert != null)
                {
                    activeAlerts.Add(alert); //adds the alert to temp list of activeAlerts

                    //Check the specific action for the alert when it becomes active
                    AlertAction alertAction = new AlertAction(alert); //pass to AlertAction class
                    switch(alert.AlertAction) {
                        case "close":
                            alertAction.closeProcess();
                            this.disposeList.Add(alert);
                            break;
                            
                        case "front":
                        case "bring to focus":
                        case "focus":
                            alertAction.bringToFront();
                            //this.disposeList.Add(alert);
                            break;
                            
                        case "none":
                            break;
                        default:
                            break;
                    }
                }
            }

            ClearDisposeListFromList();

            if (activeAlerts.Count < 1) {return null; }
            else
            {
                alertSound.play();
                return activeAlerts;
            } //return null if no alerts present
        }


        public List<Alert> PassiveAlerts()
        {
            List<Alert> passiveAlerts = new List<Alert>();

            foreach (Alert alert in this.list)
            {
                if (alert.AlertLimit > fetchTick(alert.ProcessName) && alert != null)
                {
                    passiveAlerts.Add(alert); //adds the alert to temp list of passiveAlerts
                }
            }

            ClearDisposeListFromList();

            if (passiveAlerts.Count < 1) { return null; } else { return passiveAlerts; } //return null if no alerts present

            
        }

        /**
         * ClearDisposeListFromList()
         * Action: 
         *  Removes the alerts located in disposeList from the active list
         * ETC info:
         * dispostList - a List<Alert> that contains alerts that are pending deletion
         * 
         * */
        public void ClearDisposeListFromList()
        {
            if (list != null && disposeList != null)
            if(list.Count > 0)
            if (disposeList.Count > 0){
                foreach(Alert alert in disposeList)
                {
                    removeAlert(alert);
                }
            }

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

        public bool addNewAlert(string processName, int alertTime, string alertAction)
        {
            //Add a new alert to the list
            try
            {
                this.list.Add(new Alert(processName, alertTime, alertAction));
            }
            catch (Exception ee)
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

        public void removeAlertByName(String inprocessName)
        {
            foreach (Alert alert in this.list)
            {
                if (alert.ProcessName == inprocessName)
                {
                    this.list.Remove(alert); //remove
                }
            }
        }

        public void clearAlerts()
        {
            list.Clear();
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

        public int fetchTick(string processName)
        {
            foreach (Alert alert in this.list)
            {
                if (alert.ProcessName == processName)
                {
                    return alert.CurrentCount;
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
