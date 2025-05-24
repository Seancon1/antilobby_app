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

namespace Antilobby_2 {
public partial class Form1 : Form {

  private User superUser = null;
  private Session superSession = null;
  Button alertButton = new Button();
  CursorStatus cursorStatus = new CursorStatus();
  private AutoUpdate.AutoUpdate VersionControl =
      new AutoUpdate.AutoUpdate(global.APP_RELEASE_NUM);
  AutomationFocusChangedEventHandler focusHandler = OnFocusChanged;

  /// <summary>
  /// Initializes a new instance of the <see cref="Form1"/> class.
  /// Sets up event handlers, initializes user and session objects, configures UI elements,
  /// checks for application updates, and attempts to authenticate with an existing token.
  /// </summary>
  public Form1() {
    InitializeComponent();
    // AutomationFocusChangedEventHandler focusHandler = OnFocusChanged;
    Automation.AddAutomationFocusChangedEventHandler(focusHandler);
    flowLayoutActiveAlerts.VerticalScroll.Enabled = true;
    alertButton.MouseClick +=
        AlertButton_MouseClick; // Add a handler so i can do stuff with clicks

    // flowLayoutActiveAlerts.scr

    // Create user and session
    superUser = new User();
    superSession = new Session(superUser);
    Logger superLogger = new Logger(superSession, superUser);

    // Initialize UI elements with version and IP information.
    try {
      lblUserIP.Text = $"Your IP: {MyUtils.GetIPAddress()}";
      toolStripStatuslblVersion.Text =
          VersionControl.IsOutDated() ? $"v {global.APP_RELEASE_NUM} (outdated)"
                                      : $"v {global.APP_RELEASE_NUM} (current)";
      toolStripStatuslblVersion.BackColor = VersionControl.IsOutDated()
                                                ? Color.FromArgb(255, 150, 152)
                                                : Color.FromName("Control");
      updateToolStripMenuItem.BackColor = VersionControl.IsOutDated()
                                              ? Color.FromArgb(255, 150, 152)
                                              : Color.FromName("Control");
    } catch (Exception error) {
      new Logger().SaveOfflineGeneric("null", new String[] { error.ToString() },
                                      3); // Log error if UI update fails.
    }

    // try checking existing auth token
    // Attempt to load an existing user token and authenticate.
    try {
      if (superSession.readUserToken().Length > 0) {
        superSession.setInMemoryUserToken(superSession.readUserToken());
        superLogger.doGetAuthEmail(); // Fetch user email if token is valid.
        superLogger.getSessionIDFromAPI(); // Get a session ID from the API.
      } else {
        // No token found, or token is invalid.
      }
    } catch (Exception error) {
      new Logger().SaveOfflineGeneric("null", new String[] { error.ToString() },
                                      3); // Log error during token auth.
    }

    /**
     * Populate AlertTime
     * presets. 1-30 min minutes
     * */
    // Populate the alert time dropdown with predefined minute intervals.
    for (int i = 0; i <= 55; i += 5) {
      int min = 60;
      comboBoxAlertTime.Items.Add((i == 0) ? ((1 * min) + $" ({1} min)")
                                           : ((i * min) + $" ({i} min)"));
    }
    // populate more, hours
    // Populate the alert time dropdown with predefined hour intervals.
    for (int i = 0; i <= 24; i += 2) {
      int hr = 3600;
      comboBoxAlertTime.Items.Add((i == 0) ? ((1 * hr) + $" ({1} hr)")
                                           : (i * hr) + $" ({i} hr)");
    }

    /**
     * MISC Control visibility
     */
  }

  /// <summary>
  /// Displays a status message in the application's status bar.
  /// The message is prefixed with the current timestamp.
  /// </summary>
  /// <param name="inText">The status message to display.</param>
  public void showStatus(string inText) {
    // Bug appears after prolonged use
    // putting try catch here to prevent crash
    try {
      toolStripStatusMain.ForeColor = Color.Gray;
      toolStripStatusMain.Text = "[" + DateTime.Now + "] " + inText;
    } catch (Exception error) {
      new Logger().SaveOfflineGeneric("null", new String[] { error.ToString() },
                                      3); // Log error if status update fails.
    }
  }

  // Handles the 'Exit' menu item click. Saves the session and closes the application.
  private async void exitToolStripMenuItem_Click(object sender, EventArgs e) {
    // save
    showStatus("Saving...");
    await doSessionSave(); // Ensure session data is saved before exiting.

    // exit application
    this.Close();
  }

  // Handles the 'Exit without Saving' menu item click. Closes the application without saving the current session.
  private void exitWithoutSavingToolStripMenuItem_Click(object sender,
                                                        EventArgs e) {
    global.closeWithoutSave = true; // Set flag to prevent saving on close.
    this.Close();
  }

  // Tool Tip buttons
  // Handles the 'Start' session menu item click. Starts the session timer if not already running.
  private void startToolStripMenuItem_Click(object sender, EventArgs e) {
    if (superSession == null)
      return;

    // if false
    // Start the session tracking if it's not already active.
    if (!superSession.State) {
      TimerProcesses.Start(); // Start the main process tracking timer.
      Debug.WriteLine("superSession = false. Starting tick cycle");
      MessageBox.Show("A new session (" + superSession.Id.ToString() +
                      ") has been started.");
    }
  }
  /**
   * Once a Session is stopped, it cannot be resumed. Pause functionality must
   *be used.
   *
   **/
  // Handles the 'Stop' session menu item click. Stops the current session, saves it, and resets for a new session.
  private async void stopToolStripMenuItem_Click(object sender, EventArgs e) {
    if (superSession == null)
      return;
    TimerProcesses.Stop(); // Stop the main process tracking timer.
    // force save of current session
    await doSessionSave(); // Save the stopped session's data.

    // Session will be overwritten by a new session. Effectively overwriting old
    // session with blank new one
    MessageBox.Show("Session stopped.");

    superSession = new Session(superUser); // Initialize a new session object.
  }

  // Handles the tick event for the main UI timer. Updates UI elements and manages form visibility.
  private void timer1_Tick(object sender, EventArgs e) {
    if (global.needsRestart) { // Check if application restart is pending.
      return;
    }

    // Hide the form if minimized and not already marked as hidden.
    if (this.WindowState == FormWindowState.Minimized && !global.isHidden) {
      global.isHidden = true;
      this.Hide();
      Debug.Print("Form hidden.");
    }

    // Update all interface components
    // Enable/disable session control menu items based on session state.
    if (superSession != null & superSession.State) {
      // Disable UI components
      startToolStripMenuItem.Enabled = false;
      stopToolStripMenuItem.Enabled = true;
    } else {
      // Disable UI components
      startToolStripMenuItem.Enabled = true;
      stopToolStripMenuItem.Enabled = false;
    }

    // Display the current session ID.
    lblMyInfoSessionID.Text =
        (superSession != null) ? "Session ID: \n" + superSession.Id.ToString()
                               : null;

    // Update UI elements based on login status.
    if (global.isLoggedIn && superSession != null) {
      accountPanel.Show();
      loginToolStripMenuItem.Visible = false;
      lblLogginInAccountName.Text = superSession.getInMemoryUserEmail();
      this.Text =
          "Antilobby" + " (" + superSession.getInMemoryUserEmail() + ")"; // Update form title with user email.
      btnLoginPlease.Visible = false; // Hide "Please Login" button.
      listViewProcessOverview.Enabled = true;
      saveOfflineToolStripMenuItem.Visible = true;
      saveToolStripMenuItem.Visible = true;
      startSessionToolStripMenuItem.Visible = true;
      currentSessionToolStripMenuItem.Visible = true;
      // startToolStripMenuItem.Enabled = !stopToolStripMenuItem.Enabled;
      // stopToolStripMenuItem.Enabled = !startToolStripMenuItem.Enabled;
    } else {
      // Show login prompts and disable session-specific controls if not logged in.
      saveOfflineToolStripMenuItem.Visible = false;
      saveToolStripMenuItem.Visible = false;
      startSessionToolStripMenuItem.Visible = false;
      currentSessionToolStripMenuItem.Visible = false;
      btnLoginPlease.Visible = true;
      listViewProcessOverview.Enabled = false;
      loginToolStripMenuItem.Visible = true;
      accountPanel.Hide();
    }
  }

  // Handles the tick event for the process tracking timer. Core logic for time tracking, AFK detection, auto-saving, and alert handling.
  private async void TimerProcesses_Tick(object sender, EventArgs e) {

    try {
      // If a restart is needed, clear session/user and restart the application.
      if (global.needsRestart) {
        superSession = null;
        superUser = null;
        Application.Restart();
        Environment.Exit(0); // Ensure current process exits after restart.
      }

    } catch (Exception error) {
      new Logger().SaveOfflineGeneric("null", new String[] { error.ToString() },
                                      3); // Log error during restart check.
    }

    if (superSession == null) { // If no active session, disable save option.
      saveToolStripMenuItem.Enabled = false;
      return;
    }

    // Increment session tick count if within 24-hour limit.
    if (superSession.TickCount < 86400) { // 86400 seconds = 24 hours
      superSession.incrementTick(
          global.processName); // pass current process FOR alertList
    }

    // INCREMENT GLOBAL TICKS
    // Check cursor idle status after (global.AFK_TIMER_LIMIT)
    // Handle AFK (Away From Keyboard) logic.
    if (cursorStatus.isCursorIdle(Cursor.Position.X, Cursor.Position.Y) &&
        cursorStatus.getIdleCount() >= global.AFK_TIMER_LIMIT) {
      // update UI for cursor status
      lblCursorStatus.Text = "Cursor Idle";
      // If cursor is idle, increment time for the currently focused process with a flag indicating idle time.
      superSession.processList.addAndCountOrCount(
          new ProcessItem(global.processName),
          1); // essential tick, ignore current process tick
      // Process.GetProcessesByName(this);
      // Process.
      // OnFocusChanged(this, new AutomationFocusChangedEventArgs(0,0));
    } else {
      // Handle specific case for "LockApp" process, potentially killing it if suspended.
      if (global.processName == "LockApp") {
        try {
          var process = Process.GetProcessesByName(global.processName);
          // Debug.Print("LockApp Thread State " +
          // process.First().Threads[0].ThreadState.ToString());
          // Debug.Print("Reason: " + process.First().Threads[0].ThreadState + "
          // | Details: " + process.First().Threads[0].WaitReason.ToString());
          // Debug.Print("Responding:" + process.First().Responding.ToString());
          if (process.First().Threads[0].WaitReason ==
              ThreadWaitReason.Suspended) {
            process.First().Kill(); // Kill LockApp if it's suspended.
            // Debug.Print("Killed LockApp");
          }
          // Debug.Print("Responding:" + process.First().Responding.ToString());
          // process.First().ToString();
          // this.BringToFront();
          // this.Focus();

        } catch (Exception error) {
          Debug.Print($"Process {global.processName} cannot be located." +
                      error); // Log if LockApp process cannot be found.
          global.processName = "null"; // Reset global process name.
          // new Logger().SaveOfflineGeneric("null", new String[] {
          // error.ToString() }, 3);
        }
      }

      lblCursorStatus.Text = "Cursor Active";
      // If cursor is active, increment time for the currently focused process normally.
      superSession.processList.addAndCountOrCount(new ProcessItem(
          global.processName)); // essential tick, normal function

      try {
        // Auto-saving every 5-minutes IF logged in
        if (superSession.TickCount != 0 && superSession.TickCount % 300 == 0 && // 300 ticks = 5 minutes
            superSession.hasInMemoryUserToken()) {
          showStatus("Auto Saving...");
          await superSession.processList.saveToDatabase(69); // Use flag 69 for API v3 save.
          showStatus("Auto Saved");
        }

        // Check if TickCount reached 24 hours (24hrs x 60secs x 60mins)
        // If session exceeds 24 hours, save and flag for restart.
        if (superSession.TickCount >= 86400) {
          showStatus("Auto Saving...");
          await superSession.processList.saveToDatabase(69);
          showStatus("Session limit reached (saved).");
          global.needsRestart = true; // Signal that the application should restart.
        }

      } catch (Exception error) {
        showStatus("Error Auto Saving");
        Debug.Print("Error Details: " + error); // Log auto-save errors.
        new Logger().SaveOfflineGeneric("null",
                                        new String[] { error.ToString() }, 3);
      }
    }

    // superSession.processList.refreshList(listProcesses); //link listProcesses
    // list to use the processList items

    // Update UI labels with current session info.
    lblTimeElapsed.Text = "Time Elapsed: " + superSession.TickCount;
    label1.Text = "" + global.processName; // Display current focused process.
    // showStatus(global.showstatus_value);
    toolStripStatusTime.Text = "" + DateTime.Now; // Display current time.
    saveToolStripMenuItem.Enabled = true; // Enable save menu item.

    /*
     * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Alert Handler
     * */

    // clear alerts before checking again
    flowLayoutActiveAlerts.Controls.Clear(); // Clear existing alert buttons.

    // loop through and check to see if there are any active alerts
    // check active alerts first
    // Display active alerts.
    try {
      if (!superSession.alertList.isEmpty() &&
          superSession.alertList.ActiveAlerts() != null) {
        foreach (Alert.Alert alert in superSession.alertList.ActiveAlerts()) {
          Button newButton = new Button();
          newButton.MouseClick += AlertButton_MouseClick;
          newButton.BackColor = Color.MediumSeaGreen;
          newButton.Text = "" + alert.AlertLimit + " : " + alert.CurrentCount +
                           " | " + alert.ProcessName +
                           $"({alert.AlertAction})";
          newButton.AutoSize = true;
          newButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
          newButton.TextAlign = ContentAlignment.MiddleCenter;
          newButton.Padding = new Padding(5);

          flowLayoutActiveAlerts.Controls.Add(newButton);
          // flowLayoutActiveAlerts.Controls.
        }
      }
    } catch {
      // Debug.WriteLine("ActiveAlerts not instantiated");
    }

    // then add passive alerts, alerts that arent active but still being watched
    // Display passive (non-triggered) alerts.
    if (!superSession.alertList.isEmpty() &
        superSession.alertList.PassiveAlerts() != null) {
      foreach (Alert.Alert alert in superSession.alertList.PassiveAlerts()) {
        Button newButton = new Button();
        newButton.MouseClick += AlertButton_MouseClick;
        newButton.BackColor = Color.LightGray;
        newButton.Text = "" + alert.AlertLimit + " : " + alert.CurrentCount +
                         " | " + alert.ProcessName + $"({alert.AlertAction})";
        newButton.AutoSize = true;
        newButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
        newButton.TextAlign = ContentAlignment.MiddleCenter;
        newButton.Padding = new Padding(5);

        flowLayoutActiveAlerts.Controls.Add(newButton);
      }
    }

    flowLayoutActiveAlerts.Refresh(); // Refresh the layout panel for alerts.

    /*
     * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~End Alert Handler
     * */

    // Display current memory usage in the debug toolstrip item.
    using (Process process1 = Process.GetCurrentProcess()) {
      toolStripDebug.Text = $"{process1.PrivateMemorySize64 / (1024 * 1024)}mb";
    }

    /**
     *  New test to display total counts, pass Control to AlertList class
     *UpdateControl
     *  -
     **/
    // Update the process list view if the mouse is not currently over it (to prevent flickering).
    if (!mainProcessListHasMouse) {
      await superSession.processList.UpdateControl(listViewProcessOverview);
    }
  }

  // Handles clicks on dynamically created alert buttons in the flow layout panel. Removes the clicked alert button.
  private void AlertButton_MouseClick(object sender, MouseEventArgs e) {
    // flowLayoutActiveAlerts.Controls.Remove();
    Button button = sender as Button;
    flowLayoutActiveAlerts.Controls.Remove(button); // Remove the button from the display.

    // superSession.alertList.removeAlert(button.Text.); // Placeholder for actual alert removal logic.
    // MessageBox.Show("Clicked " + button.Text);
  }

  // Placeholder for a test click event handler.
  private void Test_Click(object sender, EventArgs e) {
    throw new NotImplementedException();
  }

  /*
   * Detects that the mouse has changed focus
   * */
  // Handles the system-wide focus change event to track the currently active application.
  static private void OnFocusChanged(object sender,
                                     AutomationFocusChangedEventArgs e) {

    try {
      AutomationElement focusedElement = sender as AutomationElement;
      if (focusedElement != null) {
        int processId =
            focusedElement.Current.ProcessId; // error when closing a program
                                              // that is being focused?

        using (Process process = Process.GetProcessById(processId)) {
          // toolStripStatusMain.Text = "Working... " + DateTime.Now;

          // label4.Text = "" + process.ProcessName;
          // MessageBox.Show("" + process.ProcessName);
          // listProcesses.Items.Add("" + process.ProcessName);
          // essential for tick to increment a ProcessItem.timeViewed
          // main.currentProcessName = process.ProcessName.ToString();

          // Sneak in and add new list handling code for testing
          // adds a new ProcessItem with a defined name to the current
          // ProcessList with the assumption the item is not already defined in
          // the ProcessList addItem checks if item is located, does nothing if
          // is (so no duplicates)
          Debug.Print("Selected: " + process.ProcessName);
          global.processName =
              process.ProcessName; // Store this value in another class to
                                   // escape Exception Out of Thread

          // label1.Text = "" + process.ProcessName.ToString();
          // superSession.processList.setListObject(listProcesses);
          // superSession.processList.addItem(new
          // ProcessItem(global.processName));

          // refreshes the inserted ListBox so that it can reflect new changes
          // superSession.processList.refreshList(listProcesses);
        }
      }
    } catch (Exception error) {
      // new Logger().SaveOfflineGeneric("null", new String[] { error.ToString()
      // }, 3);
      Debug.Print("Error in OnFocusChanged: " + error.ToString()); // Log errors during focus change handling.
    }
  }

  // Placeholder for mouse click event on the process list (currently no specific logic).
  private void listProcesses_MouseClick(object sender, MouseEventArgs e) {
    // Interesting property of c#, you can cast a class for a selected ListBox
    // item
    /*
    ProcessItem item = listProcesses.SelectedItem as ProcessItem;

        if(item != null)
    {
        lblSelectedProcessName.Text = item.getName() + "" + item.getTime();
    }

    */
    /*
    if(listProcesses.SelectedItem != null)
    {

        lblSelectedProcessName.Text = listProcesses.SelectedItem.ToString();

    }
    */
  }

  // Asynchronously saves the current session data. Handles online and offline saving.
  private async Task doSessionSave() {
    try {
      if (!global.isLoggedIn) { // If not logged in, prompt to login.
        MessageBox.Show("You must login now to save sessions.");
        showStatus("Error Saving");

      } else { // If logged in, proceed with saving.
        showStatus("Saving...");
        await superSession.processList.saveToDatabase(69); // Use flag 69 for API v3 save.
        showStatus("Saved");
      }

    } catch (System.Net.WebException error) { // Handle network errors during save.
      new Logger().SaveOfflineGeneric("null", new String[] { error.ToString() },
                                      3); // Log network error.
      // MessageBox.Show("Error: " + error.ToString());
      // MessageBox.Show("Saving your session offline, your session will be
      // submitted when you are online.");
      superSession.saveSessionOffline(); // Save session data locally if online save fails.
      showStatus("Saved Offline");
    }
  }
  // Event handler for the 'Save' menu item.
  private async void saveToolStripMenuItem_Click_1(object sender, EventArgs e) {
    await doSessionSave();
  }

  // Copies the MAC address to the clipboard.
  private void mACAddresssaveToClipboardToolStripMenuItem_Click(object sender,
                                                                EventArgs e) {

    Clipboard.SetText("" + MyUtils.getMacAddress());
  }

  // Copies the IP address to the clipboard.
  private void iPAddressclipboardToolStripMenuItem_Click(object sender,
                                                         EventArgs e) {
    Clipboard.SetText("" + MyUtils.GetIPAddress());
  }

  // Copies the current session ID to the clipboard.
  private void sessionValueclipboardToolStripMenuItem_Click(object sender,
                                                            EventArgs e) {
    Clipboard.SetText("" + superSession.Id);
  }

  /*
   * An attempt to save session information before application closes.
   * */
  // Handles the form closing event to ensure session data is saved.
  private async void Form1_FormClosing(object sender, FormClosingEventArgs e) {
    if (global.needsRestart) { // If restart is pending, skip saving.
      return;
    }

    // Optional close without saving, default FALSE
    // Save session if not explicitly closing without save and session exists.
    if (!global.closeWithoutSave && superSession != null) {
      try {
        showStatus("Saving...");

        // await superSession.processList.saveToDatabase(69); //flag = 1 to set
        // state to false
        await doSessionSave(); // Perform session save.
        Debug.WriteLine("Done saving...");

      } catch (Exception error) {
        new Logger().SaveOfflineGeneric("null",
                                        new String[] { error.ToString() }, 3); // Log errors during closing save.
        if (global.needsRestart) { // If restart was triggered during save, skip message.
          return;
        } else {
          MessageBox.Show("Unable to save to online database, attempting to " +
                          "save offline... \n" + error.ToString());
        }
      }

      // If session was not active (e.g., stopped manually), confirm save.
      if (superSession.State == false) {
        MessageBox.Show("Saving complete, application will close now.");
        // this.Close(); // Form will close anyway due to FormClosing event.
      }
    }
  }

  // Handles mouse hover over the process selection combobox for alerts. Refreshes the item list.
  private void comboBox1_MouseHover(object sender, EventArgs e) {
    if (superSession.processList == null || superSession == null ||
        superSession.processList.getListOfNames() == null)
      return;

    // refresh list for alert selection of previously hovered processes
    comboBoxSelectableProcessesAlert.Items.Clear(); // Clear existing items.
    try {
      // Populate with names from the current process list.
      if (superSession.processList != null && superSession != null) {
        foreach (string name in superSession.processList.getListOfNames()) {
          comboBoxSelectableProcessesAlert.Items.Add(name);
        }
      }
    } catch (Exception error) {
      Debug.WriteLine("Error refreshing selectable process list" +
                      error.ToString()); // Log errors during refresh.
    }
  }

  // Handles the 'Add Alert' button click. Creates a new alert based on UI selections.
  private void btnAddAlert_Click(object sender, EventArgs e) {

    // Controls to look at before proceeding
    // Ensure process name and alert time are selected.
    if (comboBoxSelectableProcessesAlert.Text != null &&
        comboBoxAlertTime.Text != null && comboBoxAlertTime.Text != "") {

      var filteredTime = comboBoxAlertTime.Text;
      int submittedAlertTime = 0;

      // Trim string so we don't add (1min) to our alert limit
      // Parse the selected alert time.
      try {

        if (comboBoxAlertTime.Text.Contains(" ")) { // Handle format like "X (unit)"
          var spaceIndex = comboBoxAlertTime.Text.IndexOf(" "); // index of
                                                                // space
          filteredTime = comboBoxAlertTime.Text.Remove(
              spaceIndex, comboBoxAlertTime.Text.Length - spaceIndex);
        }

        submittedAlertTime = Convert.ToInt32(filteredTime);

        // Add to alert list
        // Add the new alert to the session's alert list.
        superSession.alertList.addNewAlert(
            comboBoxSelectableProcessesAlert.Text, submittedAlertTime,
            comboBoxAlertAction.Text);

        // display
        // Add the new alert to the UI list view for display.
        listViewCurrentAlerts.Items.Add(
            comboBoxSelectableProcessesAlert.Text + " (" +
            superSession.alertList.fetchAlertTime(
                comboBoxSelectableProcessesAlert.Text) +
            ")"); // Add to display

      } catch {
        MessageBox.Show("Unable to add alert."); // Show error if parsing or adding fails.
      }
    }
  }

  // Handles mouse clicks on the list view of current alerts. Enables the remove button if an item is focused.
  private void listViewCurrentAlerts_MouseClick(object sender,
                                                MouseEventArgs e) {

    // Enable remove and
    if (listViewCurrentAlerts.FocusedItem != null) {
      btnAlertRemove.Enabled = true; // Enable remove button when an alert is selected.
    } else {
      btnAlertRemove.Enabled = false;
    }
  }

  // Handles the 'Remove Alert' button click. Removes the selected alert from the list view.
  private void btnAlertRemove_Click(object sender, EventArgs e) {
    if (listViewCurrentAlerts.FocusedItem != null) {
      // Note: This only removes from UI. Logic to remove from superSession.alertList is missing here.
      listViewCurrentAlerts.FocusedItem.Remove();
      btnAlertRemove.Enabled = false; // Disable remove button after removal.
    }
  }

  /**
   *
   * View Tab Actions
   *
   * */

  /*
   * Visit website
   * Brings user to the website. Depending on if they are logged in or not the
   * URL is changed.
   *
   */
  // Handles the 'Website' menu item click. Opens the application's website.
  private void websiteToolStripMenuItem_Click(object sender, EventArgs e) {
    try {
      // Open user-specific session page if logged in, otherwise general site.
      if (superSession.hasInMemoryUserToken()) {
        System.Diagnostics.Process.Start(
            "https://antilobby.prestigecode.com/user/sessions");
      } else {
        System.Diagnostics.Process.Start("https://antilobby.prestigecode.com/");
      }
    } catch (Exception x) {
      MessageBox.Show("Unable to open the website." + x.ToString());
    }
  }

  // Handles the 'Current Session' menu item click. Opens the webpage for the current session ID.
  private void currentSessionToolStripMenuItem_Click(object sender,
                                                     EventArgs e) {
    try {
      System.Diagnostics.Process.Start(
          "https://antilobby.prestigecode.com/session/" + superSession.Id);
    } catch (Exception x) {
      MessageBox.Show("Unable to open the website." + x.ToString());
    }
  }

  // Placeholder for an event handler.
  private void bindingSource1_CurrentChanged(object sender, EventArgs e) {}

  // Handles the 'Clear All Alerts' button click. Clears all alerts from the session and UI.
  private void button1_Click(object sender, EventArgs e) {
    superSession.alertList.clearAlerts(); // Clear alerts from the session logic.
    listViewCurrentAlerts.Clear(); // Clear alerts from the UI list view.
  }

  // Handles the 'Save Offline' menu item click. Triggers offline save of the session.
  private void saveOfflineToolStripMenuItem_Click(object sender, EventArgs e) {
    superSession.saveSessionOffline();
  }

  // Placeholder event handler for 'Offline Storage' menu item.
  private void offlineStorageToolStripMenuItem_Click(object sender,
                                                     EventArgs e) {
    // Open offline storage interface
    // new offline_sessions().Show(); // Intended to show a form for offline sessions.
  }

  // Test method for retrieving and displaying data from offline storage.
  private void testRetrieveToolStripMenuItem_Click(object sender, EventArgs e) {
    Utils.SessionConverter sessionConverter =
        new Utils.SessionConverter(superSession.fetchOfflineStorage());
    StringBuilder stringBuilder = new StringBuilder();
    // This loop structure might be problematic if GetFirstProcessList modifies underlying state in a way
    // that makes the loop condition or item retrieval behave unexpectedly.
    // Assuming GetFirstProcessList correctly returns one session's items and updates state.
    foreach (var item in sessionConverter.GetFirstProcessList()) {
      stringBuilder.Append("" + item.getName() + " : " + item.getTime());
    }
    MessageBox.Show("" + stringBuilder.ToString());
  }

  // Handles the 'Save Storage' menu item click. Triggers saving of all offline stored sessions.
  private void saveStorageToolStripMenuItem_Click(object sender, EventArgs e) {

    superSession.saveAllOfflineStorage();
  }

  /// <summary>
  /// Opens the login window, passing the current session information.
  /// </summary>
  public void openLoginWindow() { new LoginClient(superSession).Show(); }

  // Handles the 'Login' menu item click. Opens the login window.
  private void loginToolStripMenuItem_Click(object sender, EventArgs e) {
    // Open login client and carry session over
    openLoginWindow();
  }

  // Handles the click event for the logout button (button2 on MyInfo tab).
  private void button2_Click(object sender, EventArgs e) { doLogout(); }

  // Test method for saving a default user token.
  private void tESTItemSaveToolStripMenuItem_Click(object sender, EventArgs e) {
    superSession.saveUserToken("default");
  }

  // Test method for reading a user token.
  private void tESTItemRetrieveToolStripMenuItem_Click(object sender,
                                                       EventArgs e) {
    superSession.readUserToken();
  }

  /*
   * Menu tool strips for setting AFK TIMER limit
   *
   * */
  /// <summary>
  /// Sets the AFK (Away From Keyboard) timer limit.
  /// </summary>
  /// <param name="limit">The AFK time limit in ticks (seconds).</param>
  public void setAFKTimerLimit(int limit) {
    global.AFK_TIMER_LIMIT = limit;
    MessageBox.Show($"AFK Time limit set to: {limit} ticks.");
  }

  // Event handler for setting AFK limit to 5 minutes.
  private void minutesToolStripMenuItem2_Click(object sender, EventArgs e) {
    setAFKTimerLimit(300); // 5 minutes * 60 seconds
  }

  // Event handler for setting AFK limit to 10 minutes.
  private void minutesToolStripMenuItem_Click(object sender, EventArgs e) {
    setAFKTimerLimit(600); // 10 minutes * 60 seconds
  }

  // Event handler for setting AFK limit to 30 minutes.
  private void minutesToolStripMenuItem1_Click(object sender, EventArgs e) {
    setAFKTimerLimit(1800); // 30 minutes * 60 seconds
  }

  // Event handler for setting AFK limit to 1 hour.
  private void hourToolStripMenuItem_Click(object sender, EventArgs e) {
    setAFKTimerLimit(3600); // 1 hour * 60 minutes * 60 seconds
  }

  /// <summary>
  /// Checks the version and updates current executable associated with this
  /// program IF version is out of date.
  /// </summary>
  // Checks for application updates and prompts the user if an update is available.
  private void doVersionCheck() {
    try {

      if (VersionControl.IsOutDated()) { // Check if current version is outdated.
        MessageBox.Show("Program is outdated (v" + global.APP_RELEASE_NUM +
                        "). Updating application to version (v" +
                        VersionControl.getNewVersion() + ")");
        ApplicationUpdater.Updater updater =
            new ApplicationUpdater.Updater(); // Initiate update process.
        MessageBox.Show(
            "Application was updated. Please relaunch for new changes.");
      } else {
        MessageBox.Show("You are running the most current version.");
      }
    } catch (Exception error) {
      new Logger().SaveOfflineGeneric("null", new String[] { error.ToString() },
                                      3); // Log errors during version check/update.
      MessageBox.Show("Unable to update. e:" + error.ToString());
    }
  }

  // Handles the 'Update' menu item click. Initiates the version check.
  private void updateToolStripMenuItem_Click(object sender, EventArgs e) {
    doVersionCheck();
  }

  // Test method for saving current session process list as JSON to offline storage.
  private void jSONOfFirstObjectToolStripMenuItem_Click(object sender,
                                                        EventArgs e) {
    // string json =
    // JsonConvert.SerializeObject(superSession.processList.GetFirstProcessItem());
    Logger logger = new Logger(superSession, superUser);
    string[] collection = {
      superSession.processList.ReturnEntireProcessListJSONFormat() // Get JSON string of process list.
    };
    logger.SaveOfflineGeneric("savedSession", collection, 0); // Save JSON to "savedSession" file.
    MessageBox.Show(
        "Saved session data to offline storage: \n " +
        superSession.processList.ReturnEntireProcessListJSONFormat());
  }

  // Test method for loading a session process list from a JSON string in offline storage.
  private void loadSavedSessionToolStripMenuItem_Click(object sender,
                                                       EventArgs e) {
    Logger logger = new Logger(superSession, superUser);
    superSession.processList.LoadProcessListFromJSONString(
        logger.readOfflineGeneric("savedSession", 1)); // Read and load from "savedSession" file.
    MessageBox.Show("Loaded saved session data.");
    // save empty item over session data to remove it after loading
    // logger.SaveOfflineGeneric("savedSession", new string[] {""}, 0); // Optionally clear the saved file.
  }

  // Placeholder for status strip item click event.
  private void statusStrip1_ItemClicked(object sender,
                                        ToolStripItemClickedEventArgs e) {}

  // Test method for adding dummy process data to the session.
  private void dummyDataToolStripMenuItem_Click(object sender, EventArgs e) {
    global.closeWithoutSave = true; // Prevent saving this dummy data on close.
    for (int i = 0; i < 100; i++) {
      superSession.processList.addItem(new ProcessItem("Dummy" + i, i));
    }

    showStatus("Loaded Dummy Data");
  }

  /// <summary>
  /// New online save method, POST request to our Antilobby API
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  // Handles the 'New Online Save' menu item click. Saves session data using API v3.
  private async void newOnlineSaveToolStripMenuItem_Click(object sender,
                                                          EventArgs e) {
    await superSession.processList.saveToDatabase(
        69); // using flag 69 to switch to new API
  }

  // Test method for fetching a session ID via the API.
  private void fetchSaveTokenToolStripMenuItem_Click(object sender,
                                                     EventArgs e) {
    Logger logger = new Logger(superSession, superUser);
    logger.getSessionIDFromAPI();
  }
  /// <summary>
  /// Logs out the current user. This involves clearing local session and user data,
  /// de-authorizing the client by clearing the saved token, and preparing the application for a restart.
  /// </summary>
  public void doLogout() {
    global.isLoggedIn = false;
    superSession.setInMemoryUserEmail("null"); // Clear in-memory email.
    this.Text = "Antilobby";                  // Reset form title.
    superSession.saveUserToken(""); // Clear saved user token from storage.
    MessageBox.Show("You have disconnected your account and deauthorized the " +
                    "client to send requests on your behalf.");
    superSession = null;          // Nullify current session.
    global.needsRestart = true; // Signal that the application should restart.
  }

  // Test method for saving session ID via API (likely redundant or specific test case).
  private async Task saveWTokenToolStripMenuItem_Click(object sender,
                                                       EventArgs e) {
    Logger logger = new Logger(superSession, superUser);
    await logger.doSessionIDSaveViaAPI();
  }

  // Handles click on the "Please Login" button. Opens the login window.
  private void btnLoginPlease_Click(object sender, EventArgs e) {
    new LoginClient(superSession).Show();
  }

  // Placeholder for tab page click event.
  private void tabPageMyAlerts_Click(object sender, EventArgs e) {}

  // Handles the 'Pause/Resume' menu item click. Toggles the process tracking timer.
  private void pauseToolStripMenuItem_Click(object sender, EventArgs e) {
    TimerProcesses.Enabled = !TimerProcesses.Enabled; // Toggle timer state.
    pauseToolStripMenuItem.Text = (TimerProcesses.Enabled) ? "Pause" : "Resume"; // Update menu item text.
  }

  // Handles double-click on the notification icon. Restores the form if minimized/hidden.
  private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e) {
    if (this.WindowState != FormWindowState.Normal) { // If form is not normal (e.g., minimized)
      this.Show();                                   // Show the form.
      this.WindowState = FormWindowState.Normal;     // Restore to normal state.
      global.isHidden = false;                       // Update hidden status flag.
      Debug.Print("Form shown.");
    }
  }

  // Alternate event handler for the "Please Login" button (likely a duplicate or for a different instance).
  private void btnLoginPlease_Click_1(object sender, EventArgs e) {
    openLoginWindow();
  }

  // Handles 'Print Debug Info' menu item click. Prints debug information for the process list.
  private void printDebugInfoToolStripMenuItem_Click(object sender,
                                                     EventArgs e) {
    this.superSession.processList.PrintDebugInfo();
  }

  // Handles text update in the alert action combobox. Shows a warning for experimental features.
  private void comboBoxAlertAction_TextUpdate(object sender, EventArgs e) {
    lblAlertWarning.Visible =
        (comboBoxAlertAction.Text == "focus") ? true : false; // Show warning if "focus" is selected.
    lblAlertWarning.Text = "You have selected an experimental feature. " +
                           "Please understand this may not work as intended.";
    lblAlertWarning.BackColor = Color.PeachPuff;
  }

  /**
   * Handle when the listView has a mouse over it and handle when the mouse
   * leaves. Function: Pause updating while mouse is located inside.
   * */
  private bool mainProcessListHasMouse = false; // Flag to track mouse presence over the list view.

  // Handles mouse entering the process overview list view. Pauses UI updates to this list.
  private void listViewProcessOverview_MouseEnter(object sender, EventArgs e) {
    listViewProcessOverview.Columns[0].Text = "Process (paused)"; // Indicate paused state in column header.
    mainProcessListHasMouse = true; // Set flag.
  }

  // Handles mouse leaving the process overview list view. Resumes UI updates to this list.
  private void listViewProcessOverview_MouseLeave(object sender, EventArgs e) {
    listViewProcessOverview.Columns[0].Text = "Process"; // Restore column header text.
    mainProcessListHasMouse = false; // Clear flag.
  }

  // Test method for adding dummy items to the process overview list view.
  private void addDummyInfoToolStripMenuItem_Click(object sender, EventArgs e) {
    for (int i = 0; i < 25; i++) {
      listViewProcessOverview.Items.Add(
          new ListViewItem(new[] { "DUMMY" + i, "" + i }));
    }
  }
}
}
