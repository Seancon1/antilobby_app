using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Antilobby_2
{
    public partial class LoginClient : Form
    {

        Session Session;
        String storedValue;

        internal LoginClient(Session session)
        {
            InitializeComponent();
            WebBrowser clientView = webBrowser;
            this.Session = session;
        }


        private string UserNavigated()
        {
            txtUrlStatus.Text = "[" + webBrowser.Url + "]";

            /*
             * Unsafe way to link a value to attach sessions, will implement a better way later
             * Action: When user url contains @, entire url contents will be saved to userToken for PrestigeCode account linking
             * */

            if(webBrowser.Url.ToString().Contains("@"))
            {
                Session.saveUserToken(webBrowser.Url.ToString());
                
                return "client_close";
            }

            return null;
            
        }

        private void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            string returnValue = storedValue;
            
            if (storedValue == null)
            {
                //nothing returned
            } else
            {
                switch (storedValue)
                {
                    case "client_close":
                        this.Close();
                        break;
                    default:
                        break;
                }
            }
        }

        private void lblLoginClientCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtUrlStatus_Click(object sender, EventArgs e)
        {
            global.isLoggedIn = true;
        }

        private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            storedValue = UserNavigated(); //asign value of result
        }

        private void btnLoginClientRegister_Click(object sender, EventArgs e)
        {
            //switchLoginRegister();
            try
            {
                System.Diagnostics.Process.Start("https://www.prestigecode.com/api2/register");
                this.Close();
            }
            catch (Exception x)
            {
                MessageBox.Show("Unable to open the website." + e.ToString());
            }
        }

        private void btnLoginClientLogin_Click(object sender, EventArgs e)
        {
            doLogin();
        }

        private void lblLoginClientReturnToLogin_Click(object sender, EventArgs e)
        {
            switchLoginRegister();
        }

        private void switchLoginRegister()
        {
            webBrowser.Visible = !webBrowser.Visible;
            lblLoginClientReturnToLogin.Visible = !lblLoginClientReturnToLogin.Visible;
            panLogin.Visible = !panLogin.Visible;
        }

        private async void doLogin()
        {
            var returnString = "";
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
            {
            { "email", "" + txtEmail.Text },
            { "password", "" + txtPassword.Text },
            { "device_name", "antilobby_app" },
            };

                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync("https://www.prestigecode.com/api/antilobby/sanctum/token", content);
                var responseString = await response.Content.ReadAsStringAsync();

                //response modification, probably not the best way to detect an error 
                //but I expect a string 100% of the time to be smaller than 256 chars
                returnString = (responseString.Length < 256) ? ""+responseString.ToString() : null;
                Session.setInMemoryUserToken(returnString);
                Session.saveUserToken(returnString);
                global.isLoggedIn = Session.hasInMemoryUserToken();
                Session.setInMemoryUserEmail(txtEmail.Text);
                
                // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Your Oauth token");
                if(returnString == null)
                {
                    MessageBox.Show("There was an error processing your credentials.");
                }
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(global.isLoggedIn)
            {
                this.Close();
                MessageBox.Show("Logged in successful. The client is now authorized to save data on your behalf.");
            }
        }
    }
}
