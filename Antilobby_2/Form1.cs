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
using Antilobby_2.Utils;
using Antilobby_2.ApplicationUpdater;
using Antilobby_2.AutoUpdate;
using Newtonsoft.Json;

namespace Antilobby_2
{
    public partial class Form1 : Form
    {

        private User superUser = null;
        private Session superSession = null;
        Button alertButton = new Button();
        CursorStatus cursorStatus = new CursorStatus();
        private AutoUpdate.AutoUpdate VersionControl = new AutoUpdate.AutoUpdate(global.APP_RELEASE_NUM);
        AutomationFocusChangedEventHandler focusHandler = OnFocusChanged;

        public Form1()
        {
            InitializeComponent();
            //AutomationFocusChangedEventHandler focusHandler = OnFocusChanged;
            Automation.AddAutomationFocusChangedEventHandler(focusHandler);
            flowLayoutActiveAlerts.VerticalScroll.Enabled = true;
            alertButton.MouseClick += AlertButton_MouseClick; //Add a handler so i can do stuff with clicks


            //flowLayoutActiveAlerts.scr

            // Create user and session
            superUser = new User();
            superSession = new Session(superUser);
            Logger superLogger = new Logger(superSession, superUser);
            

            try {
                lblUserIP.Text = $"Your IP: {MyUtils.GetIPAddress()}";
                toolStripStatuslblVersion.Text = VersionControl.IsOutDated() ? $"v {global.APP_RELEASE_NUM} (outdated)" : $"v {global.APP_RELEASE_NUM} (current)";
                toolStripStatuslblVersion.BackColor = VersionControl.IsOutDated() ? Color.FromArgb(255,150,152) : Color.FromName("Control");
                updateToolStripMenuItem.BackColor = VersionControl.IsOutDated() ? Color.FromArgb(255,150,152) : Color.FromName("Control");
            } catch (Exception error)
            {
                new Logger().SaveOfflineGeneric("null", new String[] { error.ToString() }, 3);
            }

            //try checking existing auth token
            try
            {
                if(superSession.readUserToken().Length > 0)
                {
                    superSession.setInMemoryUserToken(superSession.readUserToken());
                    superLogger.doGetAuthEmail();
                    superLogger.getSessionIDFromAPI();
                } else
                {

                }
            } catch (Exception error)
            {
                new Logger().SaveOfflineGeneric("null", new String[] { error.ToString() }, 3);
            }

            /**
             * Populate AlertTime 
             * presets. 1-30 min minutes
             * */
            for(int i=0; i <= 55; i+=5)
            {
                int min = 60;
                comboBoxAlertTime.Items.Add((i==0) ? ((1 * min) + $" ({1} min)") : ((i * min) + $" ({i} min)"));
            }
            //populate more, hours
            for (int i = 0; i <= 24; i+=2)
            {
                int hr = 3600;
                comboBoxAlertTime.Items.Add((i == 0) ? ((1 * hr) + $" ({1} hr)") : (i * hr) + $" ({i} hr)");
            }
            

        }

        public void showStatus(string inText)
        {
            //Bug appears after prolonged use
            //putting try catch here to prevent crash
            try {
                toolStripStatusMain.ForeColor = Color.Gray;
                toolStripStatusMain.Text = "[" + DateTime.Now + "] " + inText;
            }
            catch (Exception error)
            {
                new Logger().SaveOfflineGeneric("null", new String[] { error.ToString() }, 3);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //exit application
            this.Close();
        }

        private void exitWithoutSavingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            global.closeWithoutSave = true; // should toggle no saving
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
            if(global.needsRestart)
            {
                return;
            }

            if(this.WindowState == FormWindowState.Minimized && !global.isHidden)
            {
                global.isHidden = true;
                this.Hide();
                Debug.Print("Form hidden.");
            }

            //Update all interface components
            if(superSession != null)
            {

                //Disable UI components
                startToolStripMenuItem.Enabled = false;
                stopToolStripMenuItem.Enabled = true;

                lblMyInfoSessionID.Text = "Session ID: \n" + superSession.Id.ToString();
            } else
            {
                //Disable UI components
                startToolStripMenuItem.Enabled = true;
                stopToolStripMenuItem.Enabled = false;

                lblMyInfoSessionID.Text = "Session ID: none";
            }

            if(global.isLoggedIn && superSession != null)
            {
                accountPanel.Show();
                loginToolStripMenuItem.Visible = false;
                lblLogginInAccountName.Text = superSession.getInMemoryUserEmail();
                this.Text = "Antilobby" + " (" + superSession.getInMemoryUserEmail() + ")";
                btnLoginPlease.Visible = false;
                listProcesses.Enabled = true;
                saveOfflineToolStripMenuItem.Visible = true;
                saveToolStripMenuItem.Visible = true;
                startSessionToolStripMenuItem.Visible = true;
            }
            else
            {
                saveOfflineToolStripMenuItem.Visible = false;
                saveToolStripMenuItem.Visible = false;
                startSessionToolStripMenuItem.Visible = false;
                btnLoginPlease.Visible = true;
                listProcesses.Enabled = false;
                loginToolStripMenuItem.Visible = true;
                accountPanel.Hide();
            }

        }

        private async void TimerProcesses_Tick(object sender, EventArgs e)
        {
       
            try
            {
                if (global.needsRestart)
                {
                    superSession = null;
                    superUser = null;
                    Application.Restart();
                    Environment.Exit(0);
                }

            }
            catch (Exception error)
            {
                new Logger().SaveOfflineGeneric("null", new String[] { error.ToString() }, 3);
            }

            if (superSession == null)
            {
                saveToolStripMenuItem.Enabled = false;
                return;
            }


            if (superSession.TickCount < 86400)
            {
                superSession.incrementTick(global.processName); //pass current process FOR alertList
            }



            //INCREMENT GLOBAL TICKS 
            //Check cursor idle status after (global.AFK_TIMER_LIMIT)
            if (cursorStatus.isCursorIdle(Cursor.Position.X, Cursor.Position.Y) && cursorStatus.getIdleCount() >= global.AFK_TIMER_LIMIT)
            {
                //update UI for cursor status
                lblCursorStatus.Text = "Cursor Idle";
                superSession.processList.addAndCountOrCount(new ProcessItem(global.processName), 1); //essential tick, ignore current process tick
                //Process.GetProcessesByName(this);
                //Process.
                //OnFocusChanged(this, new AutomationFocusChangedEventArgs(0,0));
            }
            else
            {
                if(global.processName == "LockApp")
                {
                    try
                    {
                    var process = Process.GetProcessesByName(global.processName);
                    //Debug.Print("LockApp Thread State " + process.First().Threads[0].ThreadState.ToString());
                    //Debug.Print("Reason: " + process.First().Threads[0].ThreadState + " | Details: " + process.First().Threads[0].WaitReason.ToString());
                    //Debug.Print("Responding:" + process.First().Responding.ToString());
                    if (process.First().Threads[0].WaitReason == ThreadWaitReason.Suspended)
                    {
                        process.First().Kill();
                        //Debug.Print("Killed LockApp");
                    }
                    //Debug.Print("Responding:" + process.First().Responding.ToString());
                    //process.First().ToString();
                    //this.BringToFront();
                    //this.Focus();

                    }
                    catch (Exception error)
                    { Debug.Print($"Process {global.processName} cannot be located."); global.processName = "null";
                        new Logger().SaveOfflineGeneric("null", new String[] { error.ToString() }, 3);
                    }
                }



                lblCursorStatus.Text = "Cursor Active";
                superSession.processList.addAndCountOrCount(new ProcessItem(global.processName)); //essential tick, normal function

                try
                {
                    //Auto-saving every 5-minutes IF logged in
                    if (superSession.TickCount != 0 && superSession.TickCount % 300 == 0 && superSession.hasInMemoryUserToken())
                    {
                        showStatus("Auto Saving...");
                        await superSession.processList.saveToDatabase(69);
                        showStatus("Auto Saved");

                    }

                    //Check if TickCount reached 24 hours (24hrs x 60secs x 60mins)
                    if (superSession.TickCount >= 86400)
                    {
                        showStatus("Auto Saving...");
                        await superSession.processList.saveToDatabase(69);
                        showStatus("Session limit reached (saved).");
                        global.needsRestart = true;

                    }

                }
                catch (Exception error) { showStatus("Error Auto Saving"); Debug.Print("Error Details: " + error);
                    new Logger().SaveOfflineGeneric("null", new String[] { error.ToString() }, 3);
                }
            }

            superSession.processList.refreshList(listProcesses); //link listProcesses list to use the processList items

            lblTimeElapsed.Text = "Time Elapsed: " + superSession.TickCount;
            label1.Text = "" + global.processName;
            //showStatus(global.showstatus_value);
            toolStripStatusTime.Text = ""+DateTime.Now;
            saveToolStripMenuItem.Enabled = true;


            /*
             * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Alert Handler 
             * */

            //clear alerts before checking again
            flowLayoutActiveAlerts.Controls.Clear();


            //loop through and check to see if there are any active alerts
            //check active alerts first
            try
            {
                if (!superSession.alertList.isEmpty() && superSession.alertList.ActiveAlerts() != null)
                {
                    foreach (Alert.Alert alert in superSession.alertList.ActiveAlerts())
                    {
                        Button newButton = new Button();
                        newButton.MouseClick += AlertButton_MouseClick;
                        newButton.BackColor = Color.MediumSeaGreen;
                        newButton.Text = "" + alert.AlertLimit + ":" + alert.CurrentCount + "|" + alert.ProcessName + $"({alert.AlertAction})";
                        newButton.AutoSize = true;
                        newButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
                        newButton.TextAlign = ContentAlignment.MiddleCenter;
                        newButton.Padding = new Padding(5);

                        flowLayoutActiveAlerts.Controls.Add(newButton);
                        //flowLayoutActiveAlerts.Controls.
                    }
                }
            } catch
            {
                //Debug.WriteLine("ActiveAlerts not instantiated");
            }
            

            //then add passive alerts, alerts that arent active but still being watched
            if (!superSession.alertList.isEmpty() & superSession.alertList.PassiveAlerts() != null)
            {
                foreach (Alert.Alert alert in superSession.alertList.PassiveAlerts())
                {
                    Button newButton = new Button();
                    newButton.MouseClick += AlertButton_MouseClick;
                    newButton.BackColor = Color.LightGray;
                    newButton.Text = "" + alert.AlertLimit + " : " + alert.CurrentCount + " | " + alert.ProcessName + $"({alert.AlertAction})";
                    newButton.AutoSize = true;
                    newButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
                    newButton.TextAlign = ContentAlignment.MiddleCenter;
                    newButton.Padding = new Padding(5);

                    flowLayoutActiveAlerts.Controls.Add(newButton);
                }
            }

            flowLayoutActiveAlerts.Refresh();

            /*
            * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~End Alert Handler 
            * */

            using (Process process1 = Process.GetCurrentProcess())
            {
                toolStripDebug.Text = $"{process1.PrivateMemorySize64/(1024*1024)}mb";
            }
                

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
         * Detects that the mouse has changed focus
         * */
        static private void OnFocusChanged(object sender, AutomationFocusChangedEventArgs e)
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
                        Debug.Print("Selected: " + process.ProcessName);
                        global.processName = process.ProcessName; //Store this value in another class to escape Exception Out of Thread
                        
                        
                        //label1.Text = "" + process.ProcessName.ToString();
                        //superSession.processList.setListObject(listProcesses);
                        //superSession.processList.addItem(new ProcessItem(global.processName));

                        //refreshes the inserted ListBox so that it can reflect new changes
                        //superSession.processList.refreshList(listProcesses);
                    }
                }
            }
            catch (Exception error)
            {
                new Logger().SaveOfflineGeneric("null", new String[] { error.ToString() }, 3);
                Debug.Print("Error" + error.ToString());
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


        private async void saveToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try {
                if(!global.isLoggedIn)
                {
                    MessageBox.Show("You must login now to save sessions.");
                    showStatus("Error Saving");
                }
                else
                {
                    showStatus("Saving...");
                    await superSession.processList.saveToDatabase(69);
                    showStatus("Saved");
                }
                
            } catch (System.Net.WebException error)
            {
                new Logger().SaveOfflineGeneric("null", new String[] { error.ToString() }, 3);
                //MessageBox.Show("Error: " + error.ToString());
                //MessageBox.Show("Saving your session offline, your session will be submitted when you are online.");
                superSession.saveSessionOffline(); 
                showStatus("Saved Offline");
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
        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (global.needsRestart)
            {
                return;
            }

            //Optional close without saving, default FALSE
            if (!global.closeWithoutSave)
            {
                try
                {
                    //Save offline data as a precaution
                    //superSession.
                    //superSession.
                    await superSession.processList.saveToDatabase(69); //flag = 1 to set state to false

                    //Loop until saveToDatabase sets State to false, otherwise keep on trying.
                    //not exactly the best way to approach this but it works now
                    /*
                    while (superSession.State)
                    {
                        this.Enabled = false; //disables main client to prevent any other actions
                        //superSession.processList.saveToDatabase(69); //flag = 1 to set state to false
                    }
                    */
                }
                catch (Exception error)
                {
                    new Logger().SaveOfflineGeneric("null", new String[] { error.ToString() }, 3);
                    if (global.needsRestart)
                    {
                        return;
                    } else
                    {
                      MessageBox.Show("Unable to save to online database, attempting to save offline... \n" + error.ToString());
                    }
                }
            
            
                if(superSession.State == false)
                {
                    MessageBox.Show("Saving complete, application will close now.");
                    //this.Close();
                }
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
                    foreach (string name in superSession.processList.getListOfNames())
                    {
                        comboBoxSelectableProcessesAlert.Items.Add(name);
                    }

                }
            }
            catch (Exception ee)
            {
                showStatus("Error refreshing");
            }
            
        }

        private void btnAddAlert_Click(object sender, EventArgs e)
        {
        

            
            //Controls to look at before proceeding
            if(comboBoxSelectableProcessesAlert.Text != null && comboBoxAlertTime.Text != null && comboBoxAlertTime.Text != "") {

                var filteredTime = comboBoxAlertTime.Text;
                int submittedAlertTime = 0;

                //Trim string so we don't add (1min) to our alert limit
                try
                {   
        
                    if (comboBoxAlertTime.Text.Contains(" "))
                    {
                        var spaceIndex = comboBoxAlertTime.Text.IndexOf(" "); //index of space
                        filteredTime = comboBoxAlertTime.Text.Remove(spaceIndex, comboBoxAlertTime.Text.Length - spaceIndex);
                    }

                    submittedAlertTime = Convert.ToInt32(filteredTime);

                    //Add to alert list
                    superSession.alertList.addNewAlert(comboBoxSelectableProcessesAlert.Text, submittedAlertTime, comboBoxAlertAction.Text);

                    //display
                    listViewCurrentAlerts.Items.Add(comboBoxSelectableProcessesAlert.Text + " ("
                        + superSession.alertList.fetchAlertTime(comboBoxSelectableProcessesAlert.Text) + ")"); //Add to display

                }
                catch
                {
                    MessageBox.Show("Unable to add alert.");
                }
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
                System.Diagnostics.Process.Start("https://antilobby.prestigecode.com/");
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

        private void offlineStorageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Open offline storage interface
            //new offline_sessions().Show();
        }

        private void testRetrieveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utils.SessionConverter sessionConverter = new Utils.SessionConverter(superSession.fetchOfflineStorage());
            StringBuilder stringBuilder = new StringBuilder();
            foreach(var item in sessionConverter.GetFirstProcessList())
            {
                stringBuilder.Append("" + item.getName() + " : " +  item.getTime());
            }
            MessageBox.Show("" + stringBuilder.ToString());

        }

        private void saveStorageToolStripMenuItem_Click(object sender, EventArgs e)
        {

            superSession.saveAllOfflineStorage();
        }

        public void openLoginWindow()
        {
            new LoginClient(superSession).Show();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Open login client and carry session over
            openLoginWindow();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            doLogout();

        }

        private void tESTItemSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            superSession.saveUserToken("default");
        }

        private void tESTItemRetrieveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            superSession.readUserToken();
        }


        /*
         * Menu tool strips for setting AFK TIMER limit
         * 
         * */
        public void setAFKTimerLimit(int limit)
        {
            global.AFK_TIMER_LIMIT = limit;
            MessageBox.Show($"AFK Time limit set to: {limit} ticks.");
           
        }

        private void minutesToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            setAFKTimerLimit(300);
        }

        private void minutesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setAFKTimerLimit(600);
        }

        private void minutesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            setAFKTimerLimit(1800);
        }

        private void hourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setAFKTimerLimit(3600);
        }

        /// <summary>
        /// Checks the version and updates current executable associated with this program IF version is out of date.
        /// </summary>
        private void doVersionCheck()
        {
            try
            {
                
                if (VersionControl.IsOutDated())
                {
                    MessageBox.Show("Program is outdated (v" + global.APP_RELEASE_NUM + "). Updating application to version (v" + VersionControl.getNewVersion() + ")");
                    ApplicationUpdater.Updater updater = new ApplicationUpdater.Updater(); //update program
                    MessageBox.Show("Application was updated. Please relaunch for new changes.");
                }
                else
                {
                    MessageBox.Show("You are running the most current version.");
                }
            }
            catch (Exception error)
            {
                new Logger().SaveOfflineGeneric("null", new String[] { error.ToString() }, 3);
                MessageBox.Show("Unable to update. e:" + error.ToString());
            }


        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            doVersionCheck();
        }

        private void jSONOfFirstObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string json = JsonConvert.SerializeObject(superSession.processList.GetFirstProcessItem());
            Logger logger = new Logger(superSession, superUser);
            string[] collection = { superSession.processList.ReturnEntireProcessListJSONFormat() };
            logger.SaveOfflineGeneric("savedSession", collection, 0);
            MessageBox.Show("Saved session data to offline storage: \n " + superSession.processList.ReturnEntireProcessListJSONFormat());
        }

        private void loadSavedSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logger logger = new Logger(superSession, superUser);
            superSession.processList.LoadProcessListFromJSONString(logger.readOfflineGeneric("savedSession", 1));
            MessageBox.Show("Loaded saved session data.");
            //save empty item over session data to remove it after loading
            //logger.SaveOfflineGeneric("savedSession", new string[] {""}, 0);
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void dummyDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            global.closeWithoutSave = true;
            for (int i = 0; i < 100; i++)
            {
                superSession.processList.addItem(new ProcessItem("Dummy"+i, i));
            }
            
            showStatus("Loaded Dummy Data");
        }

        /// <summary>
        /// New online save method, POST request to our Antilobby API
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void newOnlineSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await superSession.processList.saveToDatabase(69); //using flag 69 to switch to new API
        }

        private void fetchSaveTokenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logger logger = new Logger(superSession, superUser);
            logger.getSessionIDFromAPI();
        }
        public void doLogout()
        {
            global.isLoggedIn = false;
            superSession.setInMemoryUserEmail("null");
            this.Text = "Antilobby";
            superSession.saveUserToken(""); //save over the saved token
            MessageBox.Show("You have disconnected your account and deauthorized the client to send requests on your behalf.");
            superSession = null;
            global.needsRestart = true;

        }

        private void saveWTokenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logger logger = new Logger(superSession, superUser);
            logger.doSessionIDSaveViaAPI();
        }

        private void btnLoginPlease_Click(object sender, EventArgs e)
        {
            new LoginClient(superSession).Show();
        }

        private void tabPageMyAlerts_Click(object sender, EventArgs e)
        {

        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TimerProcesses.Enabled = !TimerProcesses.Enabled;
            pauseToolStripMenuItem.Text = (TimerProcesses.Enabled) ? "Pause" : "Resume";
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(this.WindowState != FormWindowState.Normal)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                global.isHidden = false;
                Debug.Print("Form shown.");
            }
        }

        private void btnLoginPlease_Click_1(object sender, EventArgs e)
        {
            openLoginWindow();
        }

        private void printDebugInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.superSession.processList.PrintDebugInfo();
        }

    }
}
