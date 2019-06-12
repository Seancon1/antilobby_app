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
using System.Windows.Automation;
using System.Diagnostics;
using AntiLobby_2;

namespace Antilobby_2
{
    public partial class Form1 : Form
    {
        private User superUser = null;
        private Session superSession = null;

        public Form1()
        {
            InitializeComponent();
            AutomationFocusChangedEventHandler focusHandler = OnFocusChanged;
            Automation.AddAutomationFocusChangedEventHandler(focusHandler);

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
                MessageBox.Show("Session already exists.");
            } else
            {
                //Start new session with existing user
                superSession = new Session(superUser);
                MessageBox.Show("A new session ("+ superSession.Id.ToString() +") has been started.");
            
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

                lblMyInfoSessionID.Text = "Session ID: " + superSession.Id.ToString();
            } else
            {
                //Disable UI components
                startToolStripMenuItem.Enabled = true;
                stopToolStripMenuItem.Enabled = false;

                lblMyInfoSessionID.Text = "Session ID: none";
            }
        }

        private void TimerProcesses_Tick(object sender, EventArgs e)
        {
            //Where the process inspection goes
            //superSession.processList.
        }


        /*
         * Detects that the mouse has changed focus, time to do something
         * yes
         * */
        private void OnFocusChanged(object sender, AutomationFocusChangedEventArgs e)
        {
            try
            {
                AutomationElement focusedElement = sender as AutomationElement;
                if (focusedElement != null)
                {
                    int processId = focusedElement.Current.ProcessId; //error when closing a program that is being focused?
                    using (Process process = Process.GetProcessById(processId))
                    {

                        label4.Text = "" + process.ProcessName;

                        //essential for tick to increment a ProcessItem.timeViewed
                        //main.currentProcessName = process.ProcessName.ToString();


                        //Sneak in and add new list handling code for testing
                        //adds a new ProcessItem with a defined name to the current ProcessList
                        //with the assumption the item is not already defined in the ProcessList
                        //addItem checks if item is located, does nothing if is (so no duplicates)
                        superSession.processList.addItem(new ProcessItem(process.ProcessName));

                        //refreshes the inserted ListBox so that it can reflect new changes
                        superSession.processList.refreshList(listProcesses);
                    }
                }
            }
            catch (Exception ee)
            {
                //something bad happened
                //MessageBox.Show(ee.ToString());

            }
        }

    }
}
