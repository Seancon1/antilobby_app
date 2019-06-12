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

        public ProcessList()
        {
            //this.list.Add(new ProcessItem("test"));
            this.list = new Dictionary<string, ProcessItem>();
        }

        public ProcessList(ListBox inListBox)
        {
            this.listBox = inListBox;
        }


        public void setListObject(ListBox listBox)
        {
            this.listBox = listBox;
        }

        public void addItem(ProcessItem inItem)
        {
            if (!this.list.ContainsKey(inItem.getName()) && inItem != null)
            {
                this.list.Add(inItem.getName(), inItem);
            }
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
                    inList.Items.Add("" + item.Value.showNameFormatted());
                }
           //}
        }

        /*
        public void saveToDatabase()
        {
            Logger item = new Logger();

            //Saves to 'antilobby' MAC timeType time, date, and Session Value
            item.saveTimeTypeAsync(Form1.getMacAddress(), main.TickCount); // attempt to save information via POST to cakePHP

            //SAVES to 'antilobby_appTime'
            foreach (KeyValuePair<string, ProcessItem> itemToSave in this.list)
            {
                //iterate through list and save for the MAC address
                item.saveGameTimeAsync(Form1.getMacAddress(), itemToSave.Value.getName(), itemToSave.Value.getTime());
            }

            //MessageBox.Show("Games should be saved");
        }
        */
    }
}
