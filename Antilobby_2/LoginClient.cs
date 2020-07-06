using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
    }
}
