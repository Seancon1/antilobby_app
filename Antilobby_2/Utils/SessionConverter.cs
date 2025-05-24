using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntiLobby_2;

namespace Antilobby_2.Utils {
/// <summary>
/// Facilitates the conversion of offline session data (stored as a string) into structured <see cref="ProcessList"/> objects.
/// It can handle scenarios where multiple session data might be concatenated or stored together.
/// </summary>
class SessionConverter {

  /// <summary>
  /// Stores the raw offline session data string that is to be processed.
  /// </summary>
  public String offlineText = null;
  /// <summary>
  /// A flag used internally to manage the state of processing multiple sessions from the <see cref="offlineText"/>.
  /// Typically, a value greater than 0 indicates more data is available or being processed.
  /// </summary>
  public int flag =
      1; // flag to help determine multiple sessions in conversion storage
  /// <summary>
  /// A list to store <see cref="ProcessList"/> objects extracted from the offline session data.
  /// </summary>
  public List<ProcessList> processListStorage = new List<ProcessList>();

  /// <summary>
  /// Initializes a new instance of the <see cref="SessionConverter"/> class with the raw offline session data.
  /// </summary>
  /// <param name="input">The string containing the offline session data to be converted.</param>
  public SessionConverter(String input) {
    // Analyze offline storage before converting to one string
    this.offlineText = input;
    this.flag = 1; // Initialize flag, assuming data is present
  }

  /// <summary>
  /// Gets the current value of the internal flag, which can indicate if there's more session data to process.
  /// </summary>
  /// <returns>The current integer value of the flag.</returns>
  public int getFlag() { return this.flag; }

  /// <summary>
  /// Retrieves and removes the first list of <see cref="ProcessItem"/> objects from the internal storage.
  /// This method assumes that <see cref="processListStorage"/> has been populated (e.g., by <see cref="extractAll"/>).
  /// It updates the internal flag if no more lists are available after the operation.
  /// </summary>
  /// <returns>A list of <see cref="ProcessItem"/> from the first processed session, or an empty list if no data is available.</returns>
  public List<ProcessItem> GetFirstProcessList() {
    // Return the first list of ProcessItems

    List<ProcessItem> list = new List<ProcessItem>();

    if (processListStorage.Any()) // Check if there's anything in storage
    {
        ProcessList firstProcessList = processListStorage[0];
        if (firstProcessList != null && firstProcessList.ReturnAllItems() != null)
        {
            // Iterate safely and create new items for the returned list
            foreach (var item in new List<ProcessItem>(firstProcessList.ReturnAllItems())) // Iterate over a copy
            {
                // The original code used .pop() which modifies the list while iterating if not careful.
                // Assuming the intent is to transfer all items from the stored ProcessList to the new list.
                list.Add(new ProcessItem(item.getName(), item.getTime()));
            }
        }
        processListStorage.RemoveAt(0); // remove item from storage
    }


    // if no more lists left, set flag to 0 indicating no more left to any
    // process that uses that flag
    if (processListStorage.Count < 1) {
      flag = 0;
    }

    return list;
  }

  // extract all from offlineText
  /// <summary>
  /// Placeholder method intended to extract all session data from <see cref="offlineText"/>
  /// and populate the <see cref="processListStorage"/>.
  /// Currently, this method is not implemented.
  /// </summary>
  public void extractAll() {
    // ProcessList processList = new ProcessList(null, null); // This variable is not used effectively.
    // StringBuilder stringBuilder = new StringBuilder(); // This variable is not used.

    // extracts and adds all different sessions into storage
    // Implementation for parsing 'offlineText' and populating 'processListStorage' is missing.
  }
}
}
