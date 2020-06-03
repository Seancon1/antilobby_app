using Antilobby_2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
          
            if(this.list.Count() < 1) { return null; }

            foreach(var item in this.list)
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

        public void addAndCount(ProcessItem inItem)
        {
            if (inItem != null && inItem.getName() != "null")
            {
                if (!this.list.ContainsKey(inItem.getName()))
                {
                    this.list.Add(inItem.getName(), inItem); //add item if it isn't located inside list
                } else
                {
                    this.list[inItem.getName()].addTime(1); //Adds one second to the item if present in list already
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
            if(this.list.ContainsKey(processName)) {
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

            foreach(KeyValuePair<string, ProcessItem> item in this.list)
            {
                list.Add(item.Key);
            }

            if (list.Count > 0) { return list; } else { return null;  };
        }

        
        public void saveToDatabase(int flag = 0) 
        {
            Logger logger = new Logger(this.session, this.user);
            try
            {
                //throw new System.Net.WebException("Cannot connect");
                //Saves to 'antilobby' uses Session Value to save
                logger.saveSessionAsync(); //uses


                //SAVES to 'antilobby_appTime'
                foreach (KeyValuePair<string, ProcessItem> itemToSave in this.list)
                {
                    //iterate through list and save for the MAC address
                    logger.saveGameTimeAsync(itemToSave.Value.getName(), itemToSave.Value.getTime());
                }

            } catch (Exception e)
            {
                throw new System.Net.WebException("Cannot connect");
            }

            if(flag == 1)
            {
                this.session.State = false;
            }
            //MessageBox.Show("Games should be saved");
        }

        public ProcessItem pop()
        {
            //retrieve the first item's value, which is the ProcessItem
            ProcessItem temp = list.First().Value;
            //remove that item from the dictionary using the key
            list.Remove(list.First().Key);
            return list.First().Value;
        }
        
    }
}
