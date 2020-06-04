using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Diagnostics;
using System.Windows.Forms;
using Antilobby_2;
using Antilobby_2.Utils;

namespace AntiLobby_2
{
    /*
     * This logger class enables the REST api functions to be done and executed
     * 
     * */
    class Logger
    {

        Session session = null;
        User user = null;

        //Place where all database communication happens

        public Logger(Session inSession, User inUser)
        {
            this.session = inSession;
            this.user = inUser;
        }

        public async void saveSessionAsync()
        {
            if (this.session.Id == null)
            {
                //Form1.showStatus("unable to save");
                return;
            }
            global.showstatus_value = "Saving";


            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
            {
            { "mac", user.MAC1 },
            { "timeType", "1" },
            { "time", "" +session.TickCount },
            { "date", DateTime.Now.ToString("yyyy-MM-dd h:mm tt")},
            { "userIP", user.IP1 },
            { "sessionValue", session.Id },
            { "action", "SaveSession" }
            };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("https://prestigecode.com/projects/antilobby/antilobby.php", content);

                var responseString = await response.Content.ReadAsStringAsync();

                MessageBox.Show(responseString.ToString());

                //MessageBox.Show("Saving Done");
                
            }
            global.showstatus_value = "Saved";
        }

        public async void saveGameTimeAsync(string processName, int processTime)
        {
            if (this.session.Id == null)
            {
                //Form1.showStatus("unable to save");
                return;
            }

            global.showstatus_value = "Saving";

            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
            {
            { "appName", "" + processName.ToString() },
            { "appTime", "" + processTime },
            { "sessionValue", "" + session.Id },
            { "action", "SaveSingleGameTime" },
            };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("https://prestigecode.com/projects/antilobby/antilobby.php", content);

                var responseString = await response.Content.ReadAsStringAsync();

                //MessageBox.Show("Game Times Saved for " + processName);
                
            }
            global.showstatus_value = "Saved";
        }

        /**
         * Method made from scratch for creating and adding text to a file at specified path
         *  String fileName : name of file
         *  String saveContents :  contents to add or save to file
         *  int flag : default 0, to adjust logic
         * */
        public void SaveOfflineGeneric(String fileName, String saveContents, int flag = 0)
        {
            //Directory and File path information
            var pathFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Antilobby\\" + fileName;
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Antilobby\\";

            if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path); //Create directory
                }

            try
            {
                //Flag indicates different type of file saving, depending on the context
                switch(flag)
                            {
                                case 0:
                                    File.WriteAllText(pathFile, saveContents);
                                    break;
                                case 1:
                                    File.AppendAllText(pathFile, saveContents);
                                    break;
                            }
            } catch (Exception e)
            {
                MessageBox.Show("Unable to save offline data." + e.ToString());
            }
        }

        /**
         * Generic file reading method
         * String fileName : name of file to read
         * int flag : flag so ReadFile method can be changed with context
         * */
        public String readOfflineGeneric(String fileName, int flag = 0)
        {
            //Path to all files used in Antilobby
            var pathFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Antilobby\\" + fileName;

            StringBuilder stringBuilder = new StringBuilder();

            try
            {
                switch (flag)
                {
                    case 0:
                        stringBuilder.Append(File.ReadAllText(pathFile));
                        break;

                    default:
                        stringBuilder.Append(File.ReadAllText(pathFile));
                        break;
                }

            } catch (Exception e)
            {
                MessageBox.Show("Unable to read offline data." + e.ToString());
            }

            return stringBuilder.ToString();
        }

        public void SaveOfflineGenericDEPRECATED(String filename, String saveContents)
        {
            //Flag to check to see if the file is actually available, automatically false
            Boolean fileExists = false;

            //string saveFile = "_UserToken.antilobby";
            //string path = Path.GetTempPath() + "antilobby\\";

            /**
             * Stack overflow user Matthew Hazzard
             * https://stackoverflow.com/questions/7618132/get-locale-specific-directory-in-my-documents
             * https://stackoverflow.com/users/974058/matthew-hazzard
             * var path and
             * */
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Antilobby\\";
            //var subFolderPath = Path.Combine(path, "\\Antilobby");


            /**
             * Check if directory or file exists
             * Creates dir & file if needed
             * */
            if (!File.Exists(path + filename))
            {

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path); //Create directory
                    File.Create(path + filename); //Create file
                    fileExists = true;
                }
                else
                {
                    File.Create(path +  filename); //Create file
                    //File.c
                    //fileExists = true;

                }

            } else
            {
                fileExists = true;
            }

            //not sure how to explicitly close the file after it is being
            File.ReadAllLines(path + filename);

            //File.Close
        
                //string content = File.ReadAllText(@"C:\Users\Sean\Documents\ComputerLog\Log.txt");


            /**
             * Continue with saving contents to file
             * */
             if(fileExists) {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(saveContents);

                    try
                    {
                        //File.Open(path + filename, FileMode.Open);

                        /*
                        foreach (var item in processList)
                        {

                            stringBuilder.Append(item.getName() + ":" + item.getTime() + "|");
                        }
                        */

                        //Add content to file
                        File.AppendAllText(path + filename, stringBuilder.ToString());

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Unable to save offline data." + e.ToString());
                    }
            }
                
        }



        public void SaveOffline(Session session)
        {
            //string path = @"D:\Google Drive\#zpersonal\LOGS\Antilobby\Log.txt";

            string saveFile = "_sessionSave.txt";
            string path = Path.GetTempPath() + "antilobby\\";


            if (File.Exists(path + saveFile))
            {
                //string content = File.ReadAllText(@"C:\Users\Sean\Documents\ComputerLog\Log.txt");

                //convert processitems to 
                //get dictionary
                List<ProcessItem> processList = session.processList.ReturnAllItems();
                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.Append("!" + session.Id + "|");

                try
                {

                //File.Open(path + saveFile, FileMode.Open);

                foreach (var item in processList)
                {

                        stringBuilder.Append(item.getName() + ":" + item.getTime() + "|");
                }

                /*Encrypt or obfuscate
                *Pending... still establishing a way to implement this feature
                * for now keeping it in plaintext
                */
           
                //Add content to file
                File.AppendAllText(path + saveFile, stringBuilder.ToString());

                } catch (Exception e)
                {
                    MessageBox.Show("Unable to save offline data." + e.ToString());
                }
              
            }
            else
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path); //Create directory
                    File.Create(path + saveFile); //Create file
                }
                else
                {
                    File.Create(path + saveFile); //Create file
                }

            }
        }

        public String fetchOfflineStorage()
        {
            string saveFile = "_sessionSave.txt";
            string path = Path.GetTempPath() + "antilobby\\";

            StringBuilder stringBuilder = new StringBuilder();

            //find file
            if (File.Exists(path + saveFile)) {
                stringBuilder.Append(File.ReadAllText(path + saveFile)); //append text from file into stringbuilder
                return stringBuilder.ToString(); //return text
            } 


                return null;
        }



        /* 
            public async void saveTimeTypeAsync(string MAC, int tickCount)
        {
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
            {
            { "mac", MAC },
            { "timeType", "1" },
            { "time", "" + tickCount },
            { "date", "" + DateTime.Now.ToString("yyyy-MM-dd h:mm tt")},
            { "sessionValue", "" + main.sessionID },
            { "action", "logSessionTime" }
            };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("http://usf.prestigecode.com/srpj/antilobby.php", content);

                var responseString = await response.Content.ReadAsStringAsync();

                MessageBox.Show(responseString.ToString());

                //MessageBox.Show("Saving Done");
            }
        }

            public async void saveGameTimeAsync(string MAC, string processName, int processTime)
        {
            
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
            {
            { "mac", MAC },
            { "appName", "" + processName.ToString() },
            { "appTime", "" + processTime },
            { "sessionValue", "" + main.sessionID },
            { "action", "logGameTime" },
            };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("http://usf.prestigecode.com/srpj/antilobby.php", content);

                var responseString = await response.Content.ReadAsStringAsync();

                //MessageBox.Show("Game Times Saved for " + processName);
            }
        }

        */



    }
}
