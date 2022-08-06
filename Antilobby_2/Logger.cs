using Antilobby_2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public Logger()
        {

        }

        public Logger(Session inSession, User inUser)
        {
            session = inSession;
            user = inUser;
        }

        public async void saveSessionAsync()
        {
            if (session.Id == null)
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
            if (session.Id == null)
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
        public void SaveOfflineGeneric(String fileName, string[] saveContents, int flag = 0)
        {
            //Directory and File path information
            //User directory
            var pathFile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.Antilobby\\" + fileName;
            var pathFileErrorLogs = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.Antilobby\\logs\\error_log";
            var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.Antilobby\\";
            var errorpath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.Antilobby\\logs\\";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path); //Create directory
            }
            else if (!Directory.Exists(errorpath))
            {
                Directory.CreateDirectory(errorpath); //Create directory
            }

            try
            {
                //Flag indicates different type of file saving, depending on the context
                switch (flag)
                {
                    //Clear contents of file, replace with
                    case 0:
                        File.WriteAllText(pathFile, saveContents[0]);
                        break;

                    //Add append contents with current
                    case 1:
                        File.AppendAllText(pathFile, saveContents[0]);
                        break;

                    //Saving session info
                    case 2:
                        //Change destination to Antilobby/Sessions/
                        pathFile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.Antilobby\\Sessions\\" + fileName;

                        for (int x = 0; x < saveContents.Length; x++)
                        {
                            File.AppendAllText(pathFile, saveContents[x]);
                        }

                        break;

                    case 3:
                        File.AppendAllText(pathFileErrorLogs, $"[{DateTime.Now}] " + saveContents[0] + Environment.NewLine);
                        break;

                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Critical Error: Unable to save offline data." + e.ToString());
            }
        }

        /**
         * Generic file reading method
         * String fileName : name of file to read
         * int flag : flag so ReadFile method can be changed with context
         * */
        public String[] readOfflineGeneric(String fileName, int flag = 0)
        {
            //Path to all files used in Antilobby
            var pathFile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.Antilobby\\" + fileName;

            StringBuilder stringBuilder = new StringBuilder();

            try
            {
                switch (flag)
                {
                    case 0:
                        stringBuilder.Append(File.ReadAllText(pathFile));
                        break;
                    case 1:
                        List<string> returnString = new List<string>();

                        using (FileStream fs = File.OpenRead(pathFile))
                        {
                            using (var sr = new StreamReader(fs))
                            {
                                string line;
                                while ((line = sr.ReadLine()) != null)
                                {
                                    returnString.Add(line);
                                }
                            }
                        }

                        //Allocate size for string
                        String[] toReturn = new string[returnString.Count];
                        int count = 0;
                        foreach (String item in returnString)
                        {
                            toReturn[count++] = item;
                        }

                        return toReturn;

                    default:
                        stringBuilder.Append(File.ReadAllText(pathFile));
                        break;
                }

            }
            catch (Exception e)
            {
                //MessageBox.Show("Unable to read offline data." + e.ToString());
                Console.WriteLine("Unable to read offline data." + e.ToString()); //Print
            }

            return new String[] { stringBuilder.ToString() };
        }

        public void SaveOfflineGeneric_DEPRECATED(String filename, String saveContents)
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
                    File.Create(path + filename); //Create file
                    //File.c
                    //fileExists = true;

                }

            }
            else
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
            if (fileExists)
            {
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

                }
                catch (Exception e)
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
            if (File.Exists(path + saveFile))
            {
                stringBuilder.Append(File.ReadAllText(path + saveFile)); //append text from file into stringbuilder
                return stringBuilder.ToString(); //return text
            }


            return null;
        }

        public async Task<bool> doSessionIDSaveViaAPI()
        {
            var returnString = "";
            using (var client = new HttpClient())
            {

                var values = new Dictionary<string, string>
            {
            { "value", "blank" },
            };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
                var content = new FormUrlEncodedContent(values);
                //var response = await client.PostAsync("https://www.prestigecode.com/api/antilobby/sanctum/token", content);
                var response = await client.PostAsync("https://antilobby.prestigecode.com/user/session/update/" + session.Id + "/" + session.TickCount, content);
                var responseString = await response.Content.ReadAsStringAsync();

                //response modification, probably not the best way to detect an error 
                //but I expect a string 100% of the time to be smaller than 256 chars
                returnString = (responseString.ToString().Length <= 7) ? "" + responseString.ToString() : "error";

                if (returnString == "error")
                {
                    //MessageBox.Show("There was an error processing.");
                    Debug.WriteLine("Error saving to API: session data");
                    return false;
                }
            }
            Debug.WriteLine("Done saving to API: session data");
            return true;
        }

        public async void doGetAuthEmail()
        {
            var returnString = "";
            using (var client = new HttpClient())
            {

                var values = new Dictionary<string, string>
            {
            { "value", "blank" },
            };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
                var content = new FormUrlEncodedContent(values);
                var response = await client.GetAsync("https://antilobby.prestigecode.com/user/get");
                var responseString = await response.Content.ReadAsStringAsync();

                //response modification, probably not the best way to detect an error 
                //but I expect a string 100% of the time to be smaller than 256 chars
                returnString = (responseString.ToString().Length < 256) ? "" + responseString.ToString() : "null";
                if (returnString == "null")
                {
                    MessageBox.Show("There was an issue trying to authenticate the saved token.");
                }
                else
                {
                    session.setInMemoryUserEmail(returnString);
                    global.isLoggedIn = true;
                }

                Debug.WriteLine("Done getting auth email");

            }
        }

        /**
         * Save session apptimes to database
         * 
         * */
        public async Task<bool> DoGenericSaveViaAPI(Dictionary<string, int> itemsToSave, string processName, int processTime, int[,] specifics = null, int flag = 0)
        {
            Debug.WriteLine($"--------Process:{processName}--START-----------------------------------");
            if (itemsToSave.Count < 1) { return true; }

            var returnString = "";
            using (var client = new HttpClient())
            {

                var values = new Dictionary<string, string>()
                {
                    { "data-segment", $"{flag}" },
                };

                //var convertedList = itemToSave.Value.GetDetailedtime().ToDictionary(x => x.Key, x=>x.Value.ToString());
                var convertedList = itemsToSave.ToDictionary(x => x.Key, x => x.Value.ToString());

                foreach (KeyValuePair<string, string> pair in convertedList)
                {
                    values.Add(pair.Key, pair.Value);
                    Debug.WriteLine($" Adding key={pair.Key} with value={pair.Value} to list");
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("https://antilobby.prestigecode.com/user/apptime/v3/" + session.Id + "/" + processName + "/" + processTime, content);
                var responseString = await response.Content.ReadAsStringAsync();

                //response modification, probably not the best way to detect an error 
                //but I expect a string 100% of the time to be smaller than 256 chars
                returnString = (responseString.ToString() == "success") ? "" + responseString.ToString() : null;
                //Debug.WriteLine("Saving response " + returnString);

                if (returnString == "success")
                {
                    //MessageBox.Show("There was an error processing.");
                    //Debug.WriteLine("Done saving entries");

                }
                else
                {
                    Debug.WriteLine("Error " + returnString.ToString());
                    return false;
                }
            }
            Debug.WriteLine($"--------Process:{processName}--FINISHED-----------------------------------");

            return true;
        }

        public async void getSessionIDFromAPI()
        {
            Debug.WriteLine("Getting session ID from API");

            var returnString = "";
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
            {
            { "device_name", "antilobby_app" },
            };
                var token = (session.hasInMemoryUserToken() == true) ? session.getInMemoryUserToken() : session.readUserToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var content = new FormUrlEncodedContent(values);
                var response = await client.GetAsync("https://antilobby.prestigecode.com/user/session/id");
                var responseString = await response.Content.ReadAsStringAsync();

                Debug.WriteLine("Return string" + returnString.ToString());

                returnString = (responseString.Length < 256) ? "" + responseString.ToString() : null;
                if (returnString == null)
                {
                    //MessageBox.Show("There was an error getting a new Session ID.");
                    Debug.Print("Error getting a new Session ID.");
                    session.Id = MyUtils.getSessionID(); //get a randomly generated ID from application incase it cannot connect
                }
                else
                {
                    session.Id = returnString;
                }


            }
            Debug.WriteLine("Finished");
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
