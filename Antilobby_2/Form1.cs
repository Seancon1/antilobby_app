using Antilobby_2;
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
    public partial class Form1 : Form
    {
        private User superUser = null;
        private Session superSession = null;

        public Form1()
        {
            InitializeComponent();
            // Create user and session
            superUser = new User();
            superSession = new Session(superUser);
        }

       
    }
}
