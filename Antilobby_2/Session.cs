using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntiLobby_2;
using Antilobby_2.Utils;
using System.Diagnostics;

namespace Antilobby_2 {
/*
 * Session holds ALL information that is used to identify a session in an
 * application instance Total time, User and an ID incase we have multiple
 * sessions in an application instance Alerts also included in session
 *
 * */
/// <summary>
/// Represents a user session, tracking time, process activity, alerts, and user information.
/// </summary>
class Session {
  private string id;
  private int tickCount; // total time elapsed
  private User user;     // user associated with session
  /// <summary>
  /// Manages the list of processes tracked during this session.
  /// </summary>
  public ProcessList processList = null;
  private Logger logger = null;
  private Boolean state = true; // Represents if the session has ever been active
  /// <summary>
  /// Manages the list of alerts configured for this session.
  /// </summary>
  public Alert.AlertList alertList = null;

  /// <summary>
  /// Initializes a new instance of the <see cref="Session"/> class for the specified user.
  /// Sets up initial values, creates associated logger, process list, and alert list.
  /// Attempts to read a locally stored user token and fetch a session ID from the API.
  /// </summary>
  /// <param name="user">The user associated with this session.</param>
  public Session(User user) {
    this.tickCount = 0;
    this.user = user;
    this.id = "null"; // fill id as null for replacement shortly after creation
    this.logger = new Logger(this, user); // create a logger instance
    this.processList = new ProcessList(this, user);
    this.alertList = new Alert.AlertList(
        processList); // processList must not be null before linking, aka create
                      // processList link before calling this
    user.Token = readUserToken(); // set token of user that has been passed to
                                  // this session instance
    this.user.Token =
        readUserToken(); // gets token if located on machine locally, otherwise
                         // set null !! LOGGER instance must be called
    logger.getSessionIDFromAPI(); // this will replace null originally placed
    this.State = false;           // start in a false state (not yet active)
    this.Paused = false;          // start not paused
  }

  /// <summary>
  /// Gets or sets the unique identifier for this session.
  /// </summary>
  public string Id {
    get => this.id;
    set => this.id = value;
  }
  /// <summary>
  /// Gets or sets the total number of ticks (time units, typically seconds) elapsed in this session.
  /// </summary>
  public int TickCount {
    get => tickCount;
    set => tickCount = value;
  }
  /// <summary>
  /// Gets or sets a value indicating whether the session has been active (i.e., ticks have been incremented).
  /// True if the session has been active, false otherwise.
  /// </summary>
  public bool State {
    get => state;
    set => state = value;
  }
  /// <summary>
  /// Gets or sets a value indicating whether the session is currently paused.
  /// (Note: The property setter seems to modify 'state' instead of a dedicated 'paused' field. This might be a bug.)
  /// </summary>
  public bool Paused {
    get => state;  // Should ideally be a separate 'paused' field
    set => state = value; // Should ideally be a separate 'paused' field
  }

  /*
   * String activeProcess - string that represents the current active process
   * */

  /// <summary>
  /// Increments the session's tick count and updates associated alerts.
  /// Sets the session <see cref="State"/> to true indicating it has been active.
  /// </summary>
  /// <param name="activeProcess">The name of the currently active process, used for alert handling.</param>
  public void incrementTick(String activeProcess) {
    this.tickCount++;
    /*
     * Set State to true. This value is defined as a way to tell us if we have
     * done any incrementation ever.
     * */
    this.State = true;

    // also increment all alert counts
    if (alertList == null) {
    } else {
      /*
       * pass the activeProcess to the alertList incrementing so that AFK
       * counter will increment
       * */
      alertList.incrementAllTicks(activeProcess);
    }

    /*
     * Experimental Feature
     * Tracks apps that user interacted with and counts the amount of time they
     * are open/having process time
     * */
    /*
    try
    {
        if(processList.ReturnAllItems() != null)
        foreach(ProcessItem processItem in processList.ReturnAllItems())
        {
            var checkProcess =
    Process.GetProcessesByName(processItem.getName()).First() != null ? true:
    false; if (checkProcess)
            {
                using (var process =
    Process.GetProcessesByName(processItem.getName()).First())
                {
                    if (process != null && !process.HasExited )
                    {
                        processItem.logOpenTime();
                        //Debug.Print($"Logging OpenTime for this
    item({processItem.getName()}): Now has {processItem.getTimeOpen()} ticks");
                    }
                }
            }
        }
    }
    catch(Exception error) { Debug.Print("Error logging process name idle time.
    May have been closed" + error); }
    //add tick if process name is still open but no actively engaged
    */
  }

  /// <summary>
  /// Saves the current session data asynchronously using the associated logger.
  /// This method likely uses an older or non-API based saving mechanism.
  /// </summary>
  public void saveSession() { logger.saveSessionAsync(); }

  /// <summary>
  /// Saves the current session data offline using the associated logger.
  /// </summary>
  public void saveSessionOffline() { logger.SaveOffline(this); }

  /// <summary>
  /// Fetches offline stored session data using the associated logger.
  /// </summary>
  /// <returns>A string containing the raw offline stored data, or null if not found.</returns>
  public String fetchOfflineStorage() { return logger.fetchOfflineStorage(); }

  /// <summary>
  /// Processes all offline stored session data, converts it, and attempts to save it to the database.
  /// </summary>
  public void saveAllOfflineStorage() {
    String offlineStorage = logger.fetchOfflineStorage(); // fetch text from
                                                          // file
    SessionConverter sessionConverter =
        new SessionConverter(offlineStorage); // put text into converter
    ProcessList tempProcessList = // Renamed to avoid conflict with member
        new ProcessList(this, this.user); // create a new processList so we can
                                          // use save to db function

    while (sessionConverter.getFlag() > 0) { // Assuming getFlag indicates more data
      foreach (var item in sessionConverter.GetFirstProcessList()) {
        // check flag
        tempProcessList.addItem(new ProcessItem(item.getName(), item.getTime()));
      }
    }

    // if(everything is fine)

    tempProcessList.saveToDatabase();
  }

  /// <summary>
  /// Sets the in-memory user authentication token for this session.
  /// </summary>
  /// <param name="token">The user authentication token.</param>
  public void setInMemoryUserToken(string token) { this.user.Token = token; }
  /// <summary>
  /// Gets the in-memory user authentication token for this session.
  /// </summary>
  /// <returns>The user authentication token.</returns>
  public string getInMemoryUserToken() { return this.user.Token; }
  /// <summary>
  /// Checks if an in-memory user authentication token is present and valid (not null or "null").
  /// </summary>
  /// <returns>True if a valid token exists in memory, false otherwise.</returns>
  public bool hasInMemoryUserToken() {
    try {
      return (this.user.Token.Length < 1 || this.user.Token == "null") ? false
                                                                       : true;
    } catch {
      return false;
    }
  }

  /// <summary>
  /// Sets the in-memory user email for this session.
  /// </summary>
  /// <param name="email">The user's email address.</param>
  public void setInMemoryUserEmail(string email) { this.user.Email = email; }
  /// <summary>
  /// Gets the in-memory user email for this session.
  /// </summary>
  /// <returns>The user's email address.</returns>
  public string getInMemoryUserEmail() { return this.user.Email; }

  /// <summary>
  /// Saves the provided content (presumably a user token) to a local file "_UserToken.antilobby" for offline storage.
  /// </summary>
  /// <param name="content">The content (token) to save.</param>
  public void saveUserToken(string content) {
    this.logger.SaveOfflineGeneric("_UserToken.antilobby",
                                   new string[] { content });
  }

  /// <summary>
  /// Reads the user token from the local offline storage file "_UserToken.antilobby".
  /// </summary>
  /// <returns>The stored user token if found, or null if the file is empty or not found.</returns>
  public string readUserToken() {
    var contents = this.logger.readOfflineGeneric("_UserToken.antilobby");
    // System.Windows.Forms.MessageBox.Show("File contents:" + contents);

    if (contents == null || contents.Length < 1 || string.IsNullOrEmpty(contents[0])) { // Added null check for contents and contents[0]
      return null;
    } else {
      return contents[0];
    }
  }

  /**
   * Check device storage for token
   * 1) Find file
   * 2) Extract token
   * */
  /// <summary>
  /// Placeholder method intended to fetch a device-specific token. Currently returns "null".
  /// </summary>
  /// <returns>Currently always returns the string "null".</returns>
  public string fetchDeviceToken() { return "null"; }
}
}
