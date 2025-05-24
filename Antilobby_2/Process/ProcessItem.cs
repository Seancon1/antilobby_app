using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiLobby_2 {
public class ProcessItem {

  private string name;
  private int timeViewed;
  private int[,] TimeViewedSpecific = new int[25, 61];
  private Dictionary<string, int> DetailedTime = new Dictionary<string, int>();
  private Dictionary<string, int> DetailsToSave = new Dictionary<string, int>();
  /// <summary>
  /// Tracks the number of times this process has been observed as the foreground application or "open".
  /// </summary>
  public int TimeOpen = 0;

  /*
   * Set up so JSON can serialize the object
   * */
  /// <summary>
  /// Gets or sets the name of the process.
  /// </summary>
  public string Name {
    get => name;
    set => name = value;
  }
  /// <summary>
  /// Gets or sets the total time (in ticks or seconds) this process has been viewed or active.
  /// </summary>
  public int TimeViewed {
    get => timeViewed;
    set => timeViewed = value;
  }
  // public int[,] TimeViewedSpecific { get => TimeViewedSpecific; set =>
  // TimeViewedSpecific = value; } public int TimeOpen { get => TimeOpen; set =>
  // TimeOpen = value; }
  /* */

  /// <summary>
  /// Initializes a new instance of the <see cref="ProcessItem"/> class.
  /// </summary>
  public ProcessItem() {}

  /// <summary>
  /// Initializes a new instance of the <see cref="ProcessItem"/> class with the specified process name.
  /// TimeViewed and TimeOpen are initialized to 0.
  /// </summary>
  /// <param name="inName">The name of the process.</param>
  public ProcessItem(string inName) {
    this.name = inName;
    this.timeViewed = 0;
    this.TimeOpen = 0;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ProcessItem"/> class with the specified process name and initial time.
  /// TimeOpen is initialized to 0.
  /// </summary>
  /// <param name="inName">The name of the process.</param>
  /// <param name="time">The initial time viewed for the process.</param>
  public ProcessItem(string inName, int time) {
    this.name = inName;
    this.timeViewed = time;
    this.TimeOpen = 0;
  }

  /// <summary>
  /// Returns this instance of the <see cref="ProcessItem"/>.
  /// </summary>
  /// <returns>The current <see cref="ProcessItem"/> instance.</returns>
  public ProcessItem get() { return this; }

  /// <summary>
  /// Returns this instance of the <see cref="ProcessItem"/>.
  /// </summary>
  /// <returns>The current <see cref="ProcessItem"/> instance.</returns>
  public ProcessItem Self() { return this; }

  /// <summary>
  /// Gets the name of the process.
  /// </summary>
  /// <returns>The name of the process.</returns>
  public string getName() { return this.name; }
  /// <summary>
  /// Gets the total time viewed for the process.
  /// </summary>
  /// <returns>The total time viewed.</returns>
  public int getTime() { return this.timeViewed; }

  /// <summary>
  /// Returns a string representation of the process name and its time viewed, formatted as "name - timeViewed".
  /// </summary>
  /// <returns>A formatted string showing the process name and time viewed.</returns>
  public String showNameFormatted() {
    return $"{this.name} - {this.timeViewed}";
  }

  /**
   * Logs time, adds time to total and also logs time by the hour
   * */
  /// <summary>
  /// Adds the specified amount of time to the total time viewed for this process.
  /// Also records this time segment in a detailed dictionary, keyed by "hour:minute".
  /// </summary>
  /// <param name="timeQuantity">The amount of time (in ticks or seconds) to add.</param>
  public void addTime(int timeQuantity) {
    this.timeViewed += timeQuantity;
    this.TimeViewedSpecific[DateTime.Now.Hour, DateTime.Now.Minute] +=
        timeQuantity;
    this.SaveDetailedTime(DateTime.Now.Hour + ":" + DateTime.Now.Minute,
                          timeQuantity);
    // Debug.Print($"Logging: Total time ({TimeViewed}) with
    // {DateTime.Now.Hour}:{DateTime.Now.Minute} at
    // {this.TimeViewedSpecific[DateTime.Now.Hour, DateTime.Now.Minute]}");
  }

  /// <summary>
  /// Increments the count of how many times this process has been observed as "open" or the foreground application.
  /// </summary>
  public void logOpenTime() { this.TimeOpen += 1; }

  /// <summary>
  /// Gets the count of how many times this process has been observed as "open".
  /// </summary>
  /// <returns>The open count.</returns>
  public int getTimeOpen() { return this.TimeOpen; }

  /// <summary>
  /// Gets the 2D array storing specific time viewed data, indexed by hour and minute.
  /// Note: Direct manipulation of this array is not recommended from outside the class.
  /// </summary>
  /// <returns>A 2D integer array representing time viewed at specific hours and minutes.</returns>
  public int[,] getTimeViewedSpecific() { return this.TimeViewedSpecific; }

  /// <summary>
  /// Saves or updates a detailed time entry. If the key (e.g., "hour:minute") exists,
  /// the time is added to the existing value; otherwise, a new entry is created.
  /// </summary>
  /// <param name="key">The key for the detailed time entry (typically "hour:minute").</param>
  /// <param name="time">The amount of time to add or set for this key.</param>
  public void SaveDetailedTime(string key, int time) {
    if (this.DetailedTime.ContainsKey(key)) {
      this.DetailedTime[key] += time;
    } else {
      this.DetailedTime.Add(key, time);
    }
  }

  /**
   * Not sure if this convoluted, perhaps I can simply this logic even further?
   * Call DoSavePrep so do all functions needed before saving functions
   *
   * */
  /// <summary>
  /// Prepares and returns a list of dictionaries containing detailed time data, split for saving.
  /// This method first calls <see cref="SeperateDetailedToSavetime"/> to move data to the save queue.
  /// Then, it splits the save queue into two dictionaries.
  /// </summary>
  /// <returns>A list containing two dictionaries of detailed time data ready for saving, or an empty list if no data to save.</returns>
  public List<Dictionary<string, int>> ReturnPreparedList() {
    List<Dictionary<string, int>> collection =
        new List<Dictionary<string, int>>();
    var haveBeenSeperated = SeperateDetailedToSavetime();
    if (haveBeenSeperated && this.DetailsToSave.Any()) { // Ensure DetailsToSave is not empty
      // check if even or odd and split sections based on that, if odd, add 1 to
      // it
      var firstSectionCount = (this.DetailsToSave.Count % 2 == 1)
                             ? (this.DetailsToSave.Count / 2) +1 // Ensure first part gets the extra item if odd
                             : (this.DetailsToSave.Count) / 2;
      //var secondSectionCount = this.DetailsToSave.Count - firstSectionCount; //This was not used

      collection.Add(this.DetailsToSave.OrderBy(pair => pair.Key)
                         .Take(firstSectionCount)
                         .ToDictionary(pair => pair.Key, pair => pair.Value));
      collection.Add(this.DetailsToSave.OrderBy(pair => pair.Key)
                         .Skip(firstSectionCount)
                         //.Take(secondSectionCount) // Take the remainder
                         .ToDictionary(pair => pair.Key, pair => pair.Value));
    }

    return collection;
  }

  /// <summary>
  /// Moves detailed time data from the active <see cref="DetailedTime"/> dictionary
  /// to the <see cref="DetailsToSave"/> dictionary, preparing it for saving.
  /// The <see cref="DetailedTime"/> dictionary is cleared after this operation.
  /// </summary>
  /// <returns>True if the operation completed (even if no data was moved).</returns>
  public bool SeperateDetailedToSavetime() {
    foreach (KeyValuePair<string, int> pair in this.DetailedTime) {

      if (this.DetailsToSave.ContainsKey(pair.Key)) {
        this.DetailsToSave[pair.Key] = pair.Value;
      } else {
        this.DetailsToSave.Add(pair.Key, pair.Value);
      }
    }
    // add Detailed time to dictionary that is pending save
    this.DetailedTime.Clear(); // clear detailed time
    return true;
  }

  /// <summary>
  /// Gets the dictionary containing detailed time data that is pending save.
  /// </summary>
  /// <returns>A dictionary of time data ready to be saved.</returns>
  public Dictionary<string, int> GetDetailedToSavetime() {
    return this.DetailsToSave;
  }

  /// <summary>
  /// Clears the dictionary that holds detailed time data pending save (<see cref="DetailsToSave"/>).
  /// </summary>
  public void clearDetailedToSaveTime() {
    this.DetailsToSave.Clear();
    Debug.Print("Clearing item old save data...");
  }

  /// <summary>
  /// Prints debug information about this process item to the debug output,
  /// including its name and counts of time entries pending save versus not pending.
  /// </summary>
  public void PrintDebugDetails() {
    Debug.WriteLine($"::Item name: {this.getName()}");
    Debug.WriteLine($"::Items not pending save: {this.DetailedTime.Count}");
    Debug.WriteLine($"::Items pending save: {this.DetailsToSave.Count}");
  }
}
}
