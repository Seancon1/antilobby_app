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
        Button alertButton = new Button();

        public Form1()
        {
            InitializeComponent();

            AutomationFocusChangedEventHandler focusHandler = OnFocusChanged;
            Automation.AddAutomationFocusChangedEventHandler(focusHandler);
            flowLayoutActiveAlerts.VerticalScroll.Enabled = true;
            alertButton.MouseClick += AlertButton_MouseClick; //Add a handler so i can do stuff with clicks

            //flowLayoutActiveAlerts.scr

            // Create user and session
            superUser = new User();
            superSession = new Session(superUser);


        }

        public void showStatus(string inText)
        {
            //Bug appears after prolonged use
            //putting try catch here to prevent crash
            try {
                toolStripStatusMain.Text = inText + " " + DateTime.Now;
            }
            catch (Exception e)
            {
                //nothing for now, still investigating bug
            }
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
            if (superSession == null)
            {
                saveToolStripMenuItem.Enabled = false;
                return;
            }

            superSession.incrementTick(global.processName); //pass current process

            //Add process to processList using a ProcessItem object
            superSession.processList.addAndCount(new ProcessItem(global.processName));
            superSession.processList.refreshList(listProcesses); //link listProcesses list to use the processList items

            lblTimeElapsed.Text = "Time Elapsed: " + superSession.TickCount;
            label1.Text = "" + global.processName;
            showStatus(global.showstatus_value);
            saveToolStripMenuItem.Enabled = true;


            /*
             * Alert Handler 
             * */
            //clear alerts before checking again
            flowLayoutActiveAlerts.Controls.Clear();
            

            //loop through and check to see if there are any active alerts
            //check active alerts first
            if (!superSession.alertList.isEmpty() & superSession.alertList.ActiveAlerts() != null)
            {
                foreach(Alert.Alert alert in superSession.alertList.ActiveAlerts())
                {
                   
                    alertButton.BackColor = Color.BlueViolet;
                    alertButton.Text = "(" + alert.AlertLimit + ":" + alert.CurrentCount + ")" + alert.ProcessName;
                    flowLayoutActiveAlerts.Controls.Add(alertButton);
                }
            }

            //then add passive alerts, alerts that arent active but still being watched
            if (!superSession.alertList.isEmpty() & superSession.alertList.PassiveAlerts() != null)
            {
                foreach (Alert.Alert alert in superSession.alertList.PassiveAlerts())
                {
                    alertButton.BackColor = Color.Gray;
                    alertButton.Text = "" + alert.AlertLimit + ":" + alert.CurrentCount + "|" + alert.ProcessName;
                    flowLayoutActiveAlerts.Controls.Add(alertButton);
                }
            }

            flowLayoutActiveAlerts.Refresh();
        }

        private void AlertButton_MouseClick(object sender, MouseEventArgs e)
        {
            //flowLayoutActiveAlerts.Controls.Remove();
            Button button = sender as Button;
            flowLayoutActiveAlerts.Controls.Remove(button);
            //superSession.alertList.removeAlert(button.Text.);
            //MessageBox.Show("Clicked " + button.Text);

        }

        private void Test_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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
                        //toolStripStatusMain.Text = "Working... " + DateTime.Now;
                        
                        //label4.Text = "" + process.ProcessName;
                        //MessageBox.Show("" + process.ProcessName);
                        //listProcesses.Items.Add("" + process.ProcessName);
                        //essential for tick to increment a ProcessItem.timeViewed
                        //main.currentProcessName = process.ProcessName.ToString();


                        //Sneak in and add new list handling code for testing
                        //adds a new ProcessItem with a defined name to the current ProcessList
                        //with the assumption the item is not already defined in the ProcessList
                        //addItem checks if item is located, does nothing if is (so no duplicates)
                        
                        global.processName = process.ProcessName; //Store this value in another class to escape Exception Out of Thread

                        
                        //label1.Text = "" + process.ProcessName.ToString();
                        //superSession.processList.setListObject(listProcesses);
                        //superSession.processList.addItem(new ProcessItem(global.processName));

                        //refreshes the inserted ListBox so that it can reflect new changes
                        //superSession.processList.refreshList(listProcesses);
                    }
                }
            }
            catch (Exception ee)
            {
                //something bad happened
                //MessageBox.Show(ee.ToString());
                showStatus("Error" + ee.ToString());
            }
        }

        private void listProcesses_MouseClick(object sender, MouseEventArgs e)
        {
            //Interesting property of c#, you can cast a class for a selected ListBox item
            /*
            ProcessItem item = listProcesses.SelectedItem as ProcessItem;
            
                if(item != null)
            {
                lblSelectedProcessName.Text = item.getName() + "" + item.getTime();
            }

            */

            if(listProcesses.SelectedItem != null)
            {
                
                lblSelectedProcessName.Text = listProcesses.SelectedItem.ToString();

            }
            
        }


        private void saveToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try {
                superSession.processList.saveToDatabase();
            } catch (System.Net.WebException error)
            {
                MessageBox.Show("Error: " + error.ToString());
                MessageBox.Show("Saving your session offline, your session will be submitted when you are online.");
                superSession.saveSessionOffline();
            }

        }

        private void mACAddresssaveToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Clipboard.SetText("" + MyUtils.getMacAddress());
        }

        private void iPAddressclipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("" + MyUtils.GetIPAddress());
        }

        private void sessionValueclipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("" + superSession.Id);
        }

        /*
         * An attempt to save session information before application closes.
         * */
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            try
            {
                //ClosingForm form = new ClosingForm(); //pass session values to new form

                //Loop until saveToDatabase sets State to false, otherwise keep on trying.
                //not exactly the best way to approach this but it works now
                while (superSession.State)
                        {
                            this.Enabled = false; //disables main client to prevent any other actions
                            superSession.processList.saveToDatabase(1); //flag = 1 to set state to false
                        }
            } catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            }
            
            
            if(superSession.State == false)
            {
                MessageBox.Show("Saving complete, application will close now.");
                //this.Close();
            }
            
            
        }

        private void comboBox1_MouseHover(object sender, EventArgs e)
        {
            
            //refresh list for alert selection of previously hovered processes
            comboBoxSelectableProcessesAlert.Items.Clear();
            try
            {
                if(superSession.processList != null)
                            {
                                foreach(string name in superSession.processList.getListOfNames())
                                {
                                    comboBoxSelectableProcessesAlert.Items.Add(name);
                                }
           
                            }
            }catch (Exception ee)
            {
                showStatus("Error refreshing");
            }
            
        }

        private void btnAddAlert_Click(object sender, EventArgs e)
        {
        
            //MessageBox.Show("Time found: " + comboBoxAlertTime.Text.ToString());

            
            //Controls to look at before proceeding
            if(comboBoxSelectableProcessesAlert.Text != null && comboBoxAlertTime.Text != null && comboBoxAlertTime.Text != "") {

                int submittedAlertTime = Convert.ToInt32(comboBoxAlertTime.Text.ToString());

                //Add to alert list
                superSession.alertList.addNewAlert(comboBoxSelectableProcessesAlert.Text, submittedAlertTime);

                //display
                listViewCurrentAlerts.Items.Add(comboBoxSelectableProcessesAlert.Text + " (" 
                    + superSession.alertList.fetchAlertTime(comboBoxSelectableProcessesAlert.Text) + ")"); //Add to display
            } 
            
        }

        private void listViewCurrentAlerts_MouseClick(object sender, MouseEventArgs e)
        {

            
            //Enable remove and
            if (listViewCurrentAlerts.FocusedItem != null)
            {
                btnAlertRemove.Enabled = true;
            } else
            {
                btnAlertRemove.Enabled = false;
            }
                
            
        }

        private void btnAlertRemove_Click(object sender, EventArgs e)
        {
            if (listViewCurrentAlerts.FocusedItem != null)
            {
                listViewCurrentAlerts.FocusedItem.Remove();
                btnAlertRemove.Enabled = false;
            }
        }

        private void websiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://www.prestigecode.com/projects/antilobby/lobby.php");
            } catch (Exception x)
            {
                MessageBox.Show("Unable to open the website." + e.ToString());
            }
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            superSession.alertList.clearAlerts();
            listViewCurrentAlerts.Clear();
        }

        private void saveOfflineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            superSession.saveSessionOffline();
        }
    }
}
