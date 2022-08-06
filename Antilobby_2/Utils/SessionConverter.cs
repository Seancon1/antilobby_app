using AntiLobby_2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Antilobby_2.Utils
{
    class SessionConverter
    {

        public String offlineText = null;
        public int flag = 1; //flag to help determine multiple sessions in conversion storage
        public List<ProcessList> processListStorage = new List<ProcessList>();

        public SessionConverter(String input)
        {
            //Analyze offline storage before converting to one string
            offlineText = input;
            flag = 1;
        }

        public int getFlag()
        {
            return flag;
        }

        public List<ProcessItem> GetFirstProcessList()
        {
            //Return the first list of ProcessItems

            List<ProcessItem> list = new List<ProcessItem>();

            foreach (var item in processListStorage[0].ReturnAllItems())
            {
                ProcessItem firstElement = processListStorage[0].pop(); //remove the item from the storage and store it here

                list.Add(new ProcessItem(firstElement.getName(), firstElement.getTime())); //add item into list
            }

            processListStorage.RemoveAt(0); //remove item

            //if no more lists left, set flag to 0 indicating no more left to any process that uses that flag
            if (processListStorage.Count < 1)
            {
                flag = 0;
            }

            return list;
        }

        //extract all from offlineText
        public void extractAll()
        {
            ProcessList processList = new ProcessList(null, null);
            StringBuilder stringBuilder = new StringBuilder();


            //extracts and adds all different sessions into storage



        }


    }
}
