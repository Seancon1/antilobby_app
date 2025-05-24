using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antilobby_2.AutoUpdate {
class FetchUpdateVersion {

  /// <summary>
  /// The URL to fetch the latest version number from.
  /// </summary>
  public static string VERSION_URL =
      "https://antilobby.prestigecode.com/download/latest/version";

  /// <summary>
  /// Stores the latest version number fetched from the server.
  /// </summary>
  public double newVersionNumber;

  /// <summary>
  /// Initializes a new instance of the <see cref="FetchUpdateVersion"/> class.
  /// </summary>
  public FetchUpdateVersion() {}

  /**
   * Connects to the online version number
   * returns the result
   * */
  /// <summary>
  /// Fetches the latest version number from the server URL specified by <see cref="VERSION_URL"/>.
  /// The fetched version is stored in <see cref="newVersionNumber"/> and also returned.
  /// </summary>
  /// <returns>The latest version number as a double, or 0.0 if parsing fails.</returns>
  public double GetVersion() {
    string result = null;
    double returnVersion;
    var myClient = new WebClient();
    Stream response = myClient.OpenRead(VERSION_URL);
    // The stream data is used here.
    StreamReader streamReader = new StreamReader(response);

    result = streamReader.ReadLine();
    response.Close();

    double.TryParse(result, out returnVersion);
    StringBuilder stringBuilder = new StringBuilder();
    // stringBuilder
    this.newVersionNumber =
        returnVersion; // set class public int to this as well
    return returnVersion;
  }
}
}
