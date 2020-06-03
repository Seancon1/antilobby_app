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
        public LoginClient()
        {
            InitializeComponent();
            WebBrowser clientView = webBrowser;
            
        }

        private void UserNavigated()
        {
            txtUrlStatus.Text = "[" + webBrowser.Url + "]";
        }

        private void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            UserNavigated();
        }

        private void lblLoginClientCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtUrlStatus_Click(object sender, EventArgs e)
        {
            global.isLoggedIn = true;
        }
    }
}
