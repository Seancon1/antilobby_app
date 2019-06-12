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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //exit application
            this.Close();
        }

        //Tool Tip buttons
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(superSession != null)
            {
                MessageBox.Show("Session already started.");
            } else
            {
                MessageBox.Show("Session has been started.");
            
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (superSession != null)
            {
                //New session destroy method will be made later when session saving is finished
                superSession = null; //unlink session, keep in memory.
                    
                MessageBox.Show("Session stopped.");
            }
            else
            {
                MessageBox.Show("No session available.");

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Update all interface components
            if(superSession != null)
            {

                //Disable UI components
                startToolStripMenuItem.Enabled = false;
                stopToolStripMenuItem.Enabled = true;

                lblMyInfoSessionID.Text = "Session ID: " + superSession.Id;
            } else
            {
                //Disable UI components
                startToolStripMenuItem.Enabled = true;
                stopToolStripMenuItem.Enabled = false;

                lblMyInfoSessionID.Text = "Session ID: none";
            }
        }
    }
}
