using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Diagnostics;
using System.Windows.Forms;

namespace AntiLobby_2
{
    /*
     * This logger class enables the REST api functions to be done and executed
     * 
     * */
    class logger
    {
        public static string Name;
        public static string ID;
        public static Boolean isUserLogged = false;

        public static string MasterStartTime;

        public void WriteToFile(string programCloseTime)
        {
            time endingtime = new time();
            string path = @"D:\Google Drive\#zpersonal\LOGS\Antilobby\Log.txt";
            if (File.Exists(@"D:\Google Drive\#zpersonal\LOGS\Antilobby\Log.txt"))
            {
                //string content = File.ReadAllText(@"C:\Users\Sean\Documents\ComputerLog\Log.txt");

                string newContent = "Session: " + MasterStartTime + " to " + endingtime.getTimeNow() + " ||  Time Spend: " + main.getConvertTime(main.TickCount) + Environment.NewLine;
                //label1.Text = newContent;
                //File.WriteAllText(path, newContent);
                File.AppendAllText(path, newContent);

            }
        }

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

                var response = await client.PostAsync("https://prestigecode.com/projects/antilobby/antilobby.php", content);

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
            { "appName", "" + processName.ToString() },
            { "appTime", "" + processTime },
            { "sessionValue", "" + main.sessionID },
            { "action", "logGameTime" },
            };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("https://prestigecode.com/projects/antilobby/antilobby.php", content);

                var responseString = await response.Content.ReadAsStringAsync();

                //MessageBox.Show("Game Times Saved for " + processName);
            }
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
