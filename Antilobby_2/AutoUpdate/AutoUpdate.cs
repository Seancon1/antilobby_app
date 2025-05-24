using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Antilobby_2.AutoUpdate {
class AutoUpdate {
  /// <summary>
  /// Stores the version number fetched from the server.
  /// </summary>
  public double fetchedVersion;
  double programVersionNum;
  FetchUpdateVersion FetchUpdateVersion = new FetchUpdateVersion();

  /*
   * Grabs the version passed from the base program's hardcoded version number
   *(int) then compares it to the website version code either UPDATES or stays
   *put, depending on how they compare
   **/
  /// <summary>
  /// Initializes a new instance of the <see cref="AutoUpdate"/> class with the current program version.
  /// </summary>
  /// <param name="versionNumber">The current version number of the running application.</param>
  public AutoUpdate(double versionNumber) {

    this.programVersionNum = versionNumber;
  }

  /// <summary>
  /// Checks if the current program version is older than the version available on the server.
  /// </summary>
  /// <returns>True if the program is outdated, false otherwise.</returns>
  public bool IsOutDated() {
    // Compare hardcoded version with webversion
    if (programVersionNum < FetchUpdateVersion.GetVersion()) {
      return true;
    }
    return false;
  }

  /// <summary>
  /// Fetches the latest version number from the server and stores it in <see cref="fetchedVersion"/>.
  /// </summary>
  /// <returns>The latest version number from the server.</returns>
  public double getNewVersion() {
    this.fetchedVersion = FetchUpdateVersion.GetVersion();
    return FetchUpdateVersion.GetVersion();
  }
}
}
