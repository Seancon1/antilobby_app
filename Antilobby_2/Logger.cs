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
