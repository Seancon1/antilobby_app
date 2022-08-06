using System;
using System.Collections.Generic;
using System.Net.Http;
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
            Session = session;
        }


        private string UserNavigated()
        {
            txtUrlStatus.Text = "[" + webBrowser.Url + "]";

            /*
             * Unsafe way to link a value to attach sessions, will implement a better way later
             * Action: When user url contains @, entire url contents will be saved to userToken for PrestigeCode account linking
             * */

            if (webBrowser.Url.ToString().Contains("@"))
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
            }
            else
            {
                switch (storedValue)
                {
                    case "client_close":
                        Close();
                        break;
                    default:
                        break;
                }
            }
        }

        private void lblLoginClientCancel_Click(object sender, EventArgs e)
        {
            Close();
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
                System.Diagnostics.Process.Start("https://antilobby.prestigecode.com/register");
                Close();
            }
            catch (Exception x)
            {
                MessageBox.Show("Unable to open the website." + e.ToString());
            }
        }

        private async Task<bool> performDoLoginAsync()
        {
            lblWorking.Visible = true;
            lblWorking.Text = "Working...";
            lblWorking.Text = !await doLogin() ? "Unable to login" : "Success";
            return true;
        }

        private async void btnLoginClientLogin_Click(object sender, EventArgs e)
        {
            await performDoLoginAsync();
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


        private async Task<bool> doLogin()
        {
            //putting a new reference to logger to prevent errors when Session is null
            AntiLobby_2.Logger logger = new AntiLobby_2.Logger();
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
                var response = await client.PostAsync("https://antilobby.prestigecode.com/sanctum/token", content);
                var responseString = await response.Content.ReadAsStringAsync();

                //response modification, probably not the best way to detect an error 
                //but I expect a string 100% of the time to be smaller than 256 chars
                returnString = (responseString.Length < 256) ? "" + responseString.ToString() : null;
                Session.setInMemoryUserToken(returnString);
                Session.saveUserToken(returnString);
                //logger.SaveOfflineGeneric("_UserToken.antilobby", new string[] { returnString });

                global.isLoggedIn = Session.hasInMemoryUserToken();
                Session.setInMemoryUserEmail(txtEmail.Text);

                // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Your Oauth token");
                if (returnString == null)
                {
                    MessageBox.Show("There was an error processing your credentials.");
                    global.needsRestart = false;
                    return false;
                }
                else
                {
                    global.needsRestart = true;
                    return true;
                }


            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (global.isLoggedIn)
            {
                Close();
                MessageBox.Show("Logged in successful. The client is now authorized to save data on your behalf.");
            }
        }

        private async void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                await performDoLoginAsync();
            }

        }

    }
}
