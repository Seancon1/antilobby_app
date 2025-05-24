using Antilobby_2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AntiLobby_2 {
/// <summary>
/// Manages a list of processes being tracked, including their activity times.
/// </summary>
class ProcessList {
  private string name;
  private Dictionary<string, ProcessItem> list;
  private ProcessItem item;
  private ListBox listBox;
  private Session session;
  private User user;

  /// <summary>
  /// Initializes a new instance of the <see cref="ProcessList"/> class with a session and user context.
  /// </summary>
  /// <param name="session">The current session.</param>
  /// <param name="user">The current user.</param>
  public ProcessList(Session session, User user) {
    // this.list.Add(new ProcessItem("test"));
    this.session = session;
    this.user = user;
    this.list = new Dictionary<string, ProcessItem>();
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ProcessList"/> class with a ListBox for UI display.
  /// This constructor may be intended for simpler use cases or testing.
  /// </summary>
  /// <param name="inListBox">The ListBox to associate with this process list for display purposes.</param>
  public ProcessList(ListBox inListBox) { this.listBox = inListBox; }

  /// <summary>
  /// Returns all <see cref="ProcessItem"/> objects in the list as a single-dimensional list.
  /// </summary>
  /// <returns>A list of <see cref="ProcessItem"/> objects, or null if the list is empty.</returns>
  public List<ProcessItem> ReturnAllItems() {
    List<ProcessItem> tempList = new List<ProcessItem>();

    if (this.list.Count() < 1) {
      return null;
    }

    foreach (var item in this.list) {
      tempList.Add(
          item.Value); // get just the object ProcessItem, which is the value
    }

    return tempList;
  }

  /// <summary>
  /// Sets or updates the ListBox control associated with this process list for UI display.
  /// </summary>
  /// <param name="listBox">The ListBox control to use for displaying process items.</param>
  public void setListObject(ListBox listBox) { this.listBox = listBox; }

  /// <summary>
  /// Retrieves a <see cref="ProcessItem"/> from the list by its name.
  /// </summary>
  /// <param name="inItem">The name of the process item to retrieve.</param>
  /// <returns>The <see cref="ProcessItem"/> if found; otherwise, null.</returns>
  public ProcessItem GetProcessItem(string inItem) {
    ProcessItem processItem = null;
    if (this.list.ContainsKey(inItem) && inItem != null) {
      this.list.TryGetValue(inItem, out processItem);
    }
    return processItem;
  }

  /// <summary>
  /// Retrieves the first <see cref="ProcessItem"/> in the list.
  /// Note: The order depends on the internal dictionary's ordering, which is not guaranteed.
  /// </summary>
  /// <returns>The first <see cref="ProcessItem"/> encountered, or null if the list is empty.</returns>
  public ProcessItem GetFirstProcessItem() {
    // ProcessItem processItem = null; // This variable is not used.
    if (!this.list.Any()) return null; // Handle empty list case
    return this.list.First()
        .Value; // ProcessItem is stored in VALUE as an object, so return that
  }

  /// <summary>
  /// Adds a new <see cref="ProcessItem"/> to the list if an item with the same name does not already exist.
  /// </summary>
  /// <param name="inItem">The <see cref="ProcessItem"/> to add.</param>
  public void addItem(ProcessItem inItem) {
    if (inItem != null && !this.list.ContainsKey(inItem.getName())) { // Added null check for inItem
      this.list.Add(inItem.getName(), inItem);
    }
  }

  /// <summary>
  /// Returns the tick count (time viewed) for a process with the given name.
  /// </summary>
  /// <param name="processName">The name of the process.</param>
  /// <returns>The time viewed for the process if found; otherwise, 0.</returns>
  public int ReturnTickOf(string processName) {
    // IF true list contains name
    if (this.list.ContainsKey(processName)) {
      return this.list[processName].getTime(); // returns time of process
    }
    return 0;
  }

  /// <summary>
  /// Adds a process item to the list if it doesn't exist, or increments its time if it does.
  /// The behavior is modified by the flag parameter.
  /// </summary>
  /// <param name="inItem">The <see cref="ProcessItem"/> to add or update.</param>
  /// <param name="flag">
  /// A flag to modify behavior:
  /// If 0 (default), time is incremented for existing items.
  /// If 1, time is not incremented for existing items (item is effectively only added if new).
  /// </param>
  public void addAndCountOrCount(ProcessItem inItem, int flag = 0) {
    if (inItem != null && inItem.getName() != "null") {
      if (!this.list.ContainsKey(inItem.getName())) {
        this.list.Add(inItem.getName(),
                      inItem); // add item if it isn't located inside list
      } else {
        if (flag == 1) {
          // do nothing
        } else {
          this.list[inItem.getName()].addTime(1);
        } // Adds one second to the item if present in list already
      }
    }
  }
  /*
  public void removeItem(ProcessItem inItem)
  {
      if (this.list.Contains(inItem))
      {
          this.list.Remove(inItem);
      }
  }
  */

  /// <summary>
  /// Adds 1 unit of time to an existing process item identified by its name.
  /// If the process is not found in the list, no action is taken.
  /// </summary>
  /// <param name="processName">The name of the process to update.</param>
  public void addTimeExistingItem(string processName) {
    if (this.list.ContainsKey(processName)) {
      this.list[processName].addTime(1);
    }
  }

  /*
  public void addTimeExistingItem(ProcessItem inItem)
  {
      if (this.list.ContainsKey(inItem.getName()))
      {
          inItem.addTime(1);
      }
  }
  */

  /// <summary>
  /// Refreshes the content of the provided ListBox with the current process items.
  /// Each item is displayed using its formatted name and time.
  /// </summary>
  /// <param name="inList">The ListBox control to update.</param>
  public void refreshList(ListBox inList) {
    inList.Items.Clear();
    // inList.Items.Add("Yay");

    // if (!list.Any())
    //{
    foreach (KeyValuePair<string, ProcessItem> item in this.list) {
      // inList.Items.Add("" + item.Value.showNameFormatted());
      inList.Items.Add("" + item.Value.showNameFormatted());
    }
    //}
  }

  /// <summary>
  /// Retrieves a list of names for all process items currently in the list.
  /// </summary>
  /// <returns>A list of process names, or null if the process list is empty.</returns>
  public List<string> getListOfNames() {
    List<string> list = new List<string>();

    foreach (KeyValuePair<string, ProcessItem> item in this.list) {
      list.Add(item.Key);
    }

    if (list.Count > 0) {
      return list;
    } else {
      return null;
    };
  }

  /// <summary>
  /// Asynchronously saves the current process list data to the database via the configured logger.
  /// Behavior can vary based on the flag.
  /// </summary>
  /// <param name="flag">
  /// A flag to control saving behavior:
  /// 69: Uses the new API v3 for saving session time and detailed app times in segments.
  /// 0 (or default): Uses older methods to save session and individual game times.
  /// </param>
  /// <returns>True if saving was successful (or initiated for all items), false if a critical part of the new API save fails. Throws WebException on connection issues.</returns>
  public async Task<bool> saveToDatabase(int flag = 0) {
    Logger logger = new Logger(this.session, this.user);
    // bool testresult;
    try {
      // throw new System.Net.WebException("Cannot connect");
      // Saves to 'antilobby' uses Session Value to save
      switch (flag) {
      // new save API switch
      case 69:

        // Save session time
        if (!await logger.doSessionIDSaveViaAPI()) {
          return false;
        }

        // Then save session apptimes
        foreach (KeyValuePair<string, ProcessItem> itemToSave in this.list) {
          // Call method to seperate Details to Save
          // itemToSave.Value.SeperateDetailedToSavetime
          // split data to save here
          var preparedData = itemToSave.Value.ReturnPreparedList();
          if (preparedData.Any()) // Ensure there's data to save
          {
            Task<bool> t1 = DoSave(logger, itemToSave, preparedData.First());
            bool status = await t1;
            if (status && preparedData.Count > 1) { // Check if there's a second segment

              Task<bool> t2 = DoSave(logger, itemToSave, preparedData.Last());
              bool status2 = await t2;
              if (!status2) {
                // Debug.Print("Failed to save segment 2");
              }
              t2.Dispose();
            }
             itemToSave.Value.clearDetailedToSaveTime(); // Clear regardless of individual segment success if first was attempted
            t1.Dispose();
          }
        }
        break;

      case 0:
      default:
        logger.saveSessionAsync(); // uses

        // SAVES to 'antilobby_appTime'
        foreach (KeyValuePair<string, ProcessItem> itemToSave in this.list) {
          // iterate through list and save for the MAC address
          logger.saveGameTimeAsync(itemToSave.Value.getName(),
                                   itemToSave.Value.getTime());
        }
        break;
      }

    } catch (Exception error) {
      new Logger().SaveOfflineGeneric("null", new String[] { error.ToString() },
                                      3);
      logger.SaveOffline(this.session);
      throw new System.Net.WebException("Cannot connect");
    }
    /*
    if (flag == 1)
    {
        this.session.State = false;
    }
    */
    // MessageBox.Show("Games should be saved");
    return true;
  }

  /// <summary>
  /// Asynchronously performs a save operation for a segment of detailed time data for a specific process item.
  /// </summary>
  /// <param name="logger">The <see cref="Logger"/> instance to use for the API call.</param>
  /// <param name="itemToSave">The key-value pair representing the process item being saved.</param>
  /// <param name="sectionToSave">A dictionary containing the specific time segment data to save.</param>
  /// <param name="flag">An optional flag passed to the API (default is 0).</param>
  /// <returns>True if the API call was initiated (actual success depends on API response handled within <see cref="Logger.DoGenericSaveViaAPI"/>).</returns>
  public async Task<bool> DoSave(Logger logger,
                                 KeyValuePair<string, ProcessItem> itemToSave,
                                 Dictionary<string, int> sectionToSave,
                                 int flag = 0) {
    await logger.DoGenericSaveViaAPI(
        sectionToSave, itemToSave.Value.getName(), itemToSave.Value.getTime(),
        itemToSave.Value.getTimeViewedSpecific(), flag); // save first segment
    await Task.Delay(500); // Consider if this delay is necessary or optimal
    return true;
  }

  /// <summary>
  /// Removes and returns the first <see cref="ProcessItem"/> from the list.
  /// Note: The "first" item depends on the internal dictionary's ordering, which is not guaranteed.
  /// </summary>
  /// <returns>The removed <see cref="ProcessItem"/>, or null if the list is empty after removal (or was empty).</returns>
  public ProcessItem pop() {
    if (!list.Any()) return null; // Handle empty list
    // retrieve the first item's value, which is the ProcessItem
    ProcessItem temp = list.First().Value;
    // remove that item from the dictionary using the key
    list.Remove(list.First().Key);
    return temp; // Return the removed item
  }

  /// <summary>
  /// Converts each item that exists in the list to a JSON format and returns
  /// it as a single string with newline separators.
  /// </summary>
  /// <returns>String in JSON format, with each item on a new line.</returns>
  public string ReturnEntireProcessListJSONFormat() {
    string json = "";
    List<ProcessItem> itemsList = this.ReturnAllItems(); // Corrected variable name

    if (itemsList == null) return json; // Handle case where list is empty

    foreach (ProcessItem item in itemsList) {
      json += "\n" + JsonConvert.SerializeObject(item);
    }

    return json;
  }

  /// <summary>
  /// Loads process items into the list from an array of JSON strings.
  /// The current list is cleared before loading new items.
  /// </summary>
  /// <param name="oInput">An array of strings, where each string is a JSON representation of a <see cref="ProcessItem"/>.</param>
  public void LoadProcessListFromJSONString(string[] oInput) {

    List<ProcessItem> itemCollection = new List<ProcessItem>();

    for (int x = 0; oInput.Length > x; x++) {
      try { // Add try-catch for robust deserialization
           itemCollection.Add(JsonConvert.DeserializeObject<ProcessItem>(oInput[x]));
      } catch (JsonException ex) {
            Debug.WriteLine($"Error deserializing item at index {x}: {oInput[x]}. Exception: {ex.Message}");
            // Optionally, handle or log the error more formally
      }
    }

    // Clear current list to prevent errors from appearing when adding duplicate
    this.list.Clear();

    // iterate through and seperate data to be deserialized into items
    foreach (ProcessItem item in itemCollection) {
      if (item != null && item.Name != null) { // Ensure item and item.Name are not null
        this.list.Add(item.Name.ToString(), item);
      }
    }
  }

  /// <summary>
  /// Prints debug information for all process items in the list to the debug output.
  /// This includes the total count of items and detailed debug info for each item.
  /// </summary>
  public void PrintDebugInfo() {
    Debug.WriteLine($":Current list count: {this.list.Count}");
    foreach (KeyValuePair<string, ProcessItem> item in this.list) {
      item.Value.PrintDebugDetails();
    }
  }

  /**
   *  Pass Flow Control to this method to update contents with process list
   * items
   *
   * */
  /// <summary>
  /// Asynchronously updates the provided ListView control with the current process items.
  /// Clears existing items and repopulates the ListView.
  /// </summary>
  /// <param name="panel">The ListView control to update.</param>
  /// <returns>True once the update is complete.</returns>
  public async Task<bool> UpdateControl(ListView panel) {
    /*
    Button button = sender as Button;
    flowLayoutActiveAlerts.Controls.Remove(button);
    */
    panel.Columns[0].Width = 135;
    panel.Columns[1].Width = 65;
    panel.Items.Clear();
    foreach (var item in list) { // Consider thread safety if 'list' can be modified elsewhere
      panel.Items.Add(new ListViewItem(
          new[] { "" + item.Value.Name, "" + item.Value.TimeViewed }));
    }

    return true;
  }
}
}
