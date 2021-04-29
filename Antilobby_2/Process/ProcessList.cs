using Antilobby_2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AntiLobby_2
{
    class ProcessList
    {
        private string name;
        private Dictionary<string, ProcessItem> list;
        private ProcessItem item;
        private ListBox listBox;
        private Session session;
        private User user;

        public ProcessList(Session session, User user)
        {
            //this.list.Add(new ProcessItem("test"));
            this.session = session;
            this.user = user;
            this.list = new Dictionary<string, ProcessItem>();
        }


        public ProcessList(ListBox inListBox)
        {
            this.listBox = inListBox;
        }


        //return items in this list as a single dimension list for easier iteration
        public List<ProcessItem> ReturnAllItems()
        {
            List<ProcessItem> tempList = new List<ProcessItem>();

            if (this.list.Count() < 1) { return null; }

            foreach (var item in this.list)
            {
                tempList.Add(item.Value); //get just the object ProcessItem, which is the value
            }

            return tempList;
        }

        public void setListObject(ListBox listBox)
        {
            this.listBox = listBox;
        }

        public ProcessItem GetProcessItem(string inItem)
        {
            ProcessItem processItem = null;
            if (this.list.ContainsKey(inItem) && inItem != null)
            {
                this.list.TryGetValue(inItem, out processItem);
            }
            return processItem;
        }

        public ProcessItem GetFirstProcessItem()
        {
            ProcessItem processItem = null;
            return this.list.First().Value; //ProcessItem is stored in VALUE as an object, so return that
        }

        public void addItem(ProcessItem inItem)
        {
            if (!this.list.ContainsKey(inItem.getName()) && inItem != null)
            {
                this.list.Add(inItem.getName(), inItem);
            }
        }

        public int ReturnTickOf(string processName)
        {
            //IF true list contains name
            if (this.list.ContainsKey(processName))
            {
                return this.list[processName].getTime(); //returns time of process
            }
            return 0;
        }
        /// <summary>
        /// Increment the tick to the current process. Flag indicates a change of logic.
        /// </summary>
        /// <param name="inItem"></param>
        /// <param name="flag"></param>
        public void addAndCountOrCount(ProcessItem inItem, int flag = 0)
        {
            if (inItem != null && inItem.getName() != "null")
            {
                if (!this.list.ContainsKey(inItem.getName()))
                {
                    this.list.Add(inItem.getName(), inItem); //add item if it isn't located inside list
                }
                else
                {
                    if (flag == 1)
                    {
                        //do nothing
                    }
                    else { this.list[inItem.getName()].addTime(1); } //Adds one second to the item if present in list already
                }

            }

        }
        /*
        public void removeItem(ProcessItem inItem)
        {
            if (this.list.Contains(inItem))
            {
                this.list.Remove(inItem);
            }
        }
        */

        public void addTimeExistingItem(string processName)
        {
            if (this.list.ContainsKey(processName))
            {
                this.list[processName].addTime(1);
            }
        }

        /*
        public void addTimeExistingItem(ProcessItem inItem)
        {
            if (this.list.ContainsKey(inItem.getName()))
            {
                inItem.addTime(1);
            }
        }
        */

        public void refreshList(ListBox inList)
        {
            inList.Items.Clear();
            //inList.Items.Add("Yay");

            //if (!list.Any())
            //{
            foreach (KeyValuePair<string, ProcessItem> item in this.list)
            {
                //inList.Items.Add("" + item.Value.showNameFormatted());
                inList.Items.Add("" + item.Value.showNameFormatted());
            }
            //}
        }

        public List<string> getListOfNames()
        {
            List<string> list = new List<string>();

            foreach (KeyValuePair<string, ProcessItem> item in this.list)
            {
                list.Add(item.Key);
            }

            if (list.Count > 0) { return list; } else { return null; };
        }


        public async Task<bool> saveToDatabase(int flag = 0)
        {
            Logger logger = new Logger(this.session, this.user);
           // bool testresult;
            try
            {
                //throw new System.Net.WebException("Cannot connect");
                //Saves to 'antilobby' uses Session Value to save
                switch (flag)
                {
                    //new save API switch
                    case 69:
                        
                        //Save session time
                        if (!await logger.doSessionIDSaveViaAPI())
                        {
                            return false;
                        }
                            

                        //Then save session apptimes
                        foreach (KeyValuePair<string, ProcessItem> itemToSave in this.list)
                        {
                            //Call method to seperate Details to Save
                            //itemToSave.Value.SeperateDetailedToSavetime
                            //split data to save here

                            Task<bool> t1 = DoSave(logger, itemToSave, itemToSave.Value.ReturnPreparedList().First());
                            bool status = await t1;
                            if (status)
                            {

                                Task<bool> t2 = DoSave(logger, itemToSave, itemToSave.Value.ReturnPreparedList().Last());
                                bool status2 = await t2;
                                if (!status2)
                                {
                                    //Remove items from list if successfully saved
                                    //Debug.Print("Failed to save segment 2");
                                    //itemToSave.Value.clearDetailedTime();
                                    //return false;
                                }
                                itemToSave.Value.clearDetailedToSaveTime();
                                t2.Dispose();
                                
                                //t2 = null; //check to see if this clears memory?

                                //return true;
                            } else
                            {
                                //Debug.Print("Failed to save segment 1");
                                //return false;
                            }
                            //t1 = null; //check to see if this clears memory?
                            t1.Dispose();
                        }

                        break;

                    case 0:
                    default:
                        logger.saveSessionAsync(); //uses


                        //SAVES to 'antilobby_appTime'
                        foreach (KeyValuePair<string, ProcessItem> itemToSave in this.list)
                        {
                            //iterate through list and save for the MAC address
                            logger.saveGameTimeAsync(itemToSave.Value.getName(), itemToSave.Value.getTime());
                        }
                        break;
                }


            }
            catch (Exception error)
            {
                new Logger().SaveOfflineGeneric("null", new String[] { error.ToString() }, 3);
                logger.SaveOffline(this.session);
                throw new System.Net.WebException("Cannot connect");
            }
            /*
            if (flag == 1)
            {
                this.session.State = false;
            }
            */
            //MessageBox.Show("Games should be saved");
            return true;
        }

        public async Task<bool> DoSave(Logger logger, KeyValuePair<string, ProcessItem> itemToSave, Dictionary<string, int> sectionToSave,  int flag=0)
        {
                await logger.DoGenericSaveViaAPI(sectionToSave, itemToSave.Value.getName(), itemToSave.Value.getTime(), itemToSave.Value.getTimeViewedSpecific(), flag); //save first segment
                await Task.Delay(500);
            return true;
        }


        public ProcessItem pop()
        {
            //retrieve the first item's value, which is the ProcessItem
            ProcessItem temp = list.First().Value;
            //remove that item from the dictionary using the key
            list.Remove(list.First().Key);
            return list.First().Value;
        }

        /// <summary>
        /// Converts each item that exists in the list to a json format and returns it.
        /// </summary>
        /// <returns>String in JSON format</returns>
        public string ReturnEntireProcessListJSONFormat()
        {
            string json = "";
            List<ProcessItem> list = this.ReturnAllItems();

            foreach (ProcessItem item in list)
            {
                json += "\n" + JsonConvert.SerializeObject(item);
            }

            return json;
        }


        public void LoadProcessListFromJSONString(string[] oInput)
        {

            List<ProcessItem> itemCollection = new List<ProcessItem>();

            for (int x = 0; oInput.Length > x; x++)
            {
                itemCollection.Add(JsonConvert.DeserializeObject<ProcessItem>(oInput[x]));
            }

            //Clear current list to prevent errors from appearing when adding duplicate
            this.list.Clear();

            //iterate through and seperate data to be deserialized into items
            foreach (ProcessItem item in itemCollection)
            {
                if (item != null)
                {
                    this.list.Add(item.Name.ToString(), item);
                }

            }
        }

        public void PrintDebugInfo()
        {
            Debug.WriteLine($":Current list count: {this.list.Count}");
            foreach (KeyValuePair<string, ProcessItem> item in this.list)
            {
                item.Value.PrintDebugDetails();
            }
        }

        /**
 *  Pass Flow Control to this method to update contents with process list items
 * 
 * */
        public async Task<bool> UpdateControl(ListView panel)
        {
            /*
            Button button = sender as Button;
            flowLayoutActiveAlerts.Controls.Remove(button);
            */
            panel.Columns[0].Width = 135;
            panel.Columns[1].Width = 65;
            panel.Items.Clear();
            foreach (var item in list)
            {
                panel.Items.Add(new ListViewItem(new[] { ""+ item.Value.Name, ""+item.Value.TimeViewed }));
            }

            return true;
        }

    }
}
