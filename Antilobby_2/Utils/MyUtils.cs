using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AntiLobby_2 {
/// <summary>
/// Provides utility methods for retrieving system and network information.
/// </summary>
public class MyUtils {
  // Private constructor to prevent instantiation of this utility class.
  // All members are static.
  MyUtils() {}

  /// <summary>
  /// Gets the local IP address of the host machine.
  /// Iterates through the host entry's address list and returns the first one found.
  /// </summary>
  /// <returns>The local IP address as a string, or "0.0.0.0" if an error occurs.</returns>
  public static String getIP() {
    string myIP = "0.0.0.0";
    try {
      string hostName = Dns.GetHostName(); // Retrive the Name of HOST
      myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
    } catch (Exception exception) {
      Console.WriteLine("Unable to getIP. " + exception.ToString());
    }
    return myIP;
  }

  /// <summary>
  /// Gets the public IP address of the machine by making a request to an external service.
  /// </summary>
  /// <returns>The public IP address as a string, or "null" if an error occurs or the address cannot be retrieved.</returns>
  public static string GetIPAddress() {
    String address = "null";

    try {

      WebRequest request =
          WebRequest.Create("https://www.prestigecode.com/get/myip");
      using (WebResponse response = request.GetResponse()) using (
          StreamReader stream =
              new StreamReader(response.GetResponseStream())) {
        address = stream.ReadToEnd();
      }

    } catch (Exception exception) {
      Console.WriteLine("Unable to GetIPAddress. " + exception.ToString());
    }

    return address;
  }

  /// <summary>
  /// Generates a random 16-digit session ID string.
  /// </summary>
  /// <returns>A string representing a 16-digit random session ID.</returns>
  public static String getSessionID() {
    // Random rand = new Random(DateTime.Now.Millisecond);
    // return rand.Next().ToString();

    Random random = new Random();
    StringBuilder id = new StringBuilder();

    while (id.Length < 16) {
      id.Append(random.Next(0, 9));
    }

    return id.ToString();
  }

  /*
   *
   * Fetched from stackoverflow, saved here incase needed
   * */
  /// <summary>
  /// Gets the MAC address of the network interface with the highest speed.
  /// </summary>
  /// <returns>The MAC address as a string, or an empty string if an error occurs or no suitable MAC address is found.</returns>
  public static string getMacAddress() {
    const int MIN_MAC_ADDR_LENGTH = 12;
    string macAddress = string.Empty;
    long maxSpeed = -1;

    try {
      // Got this from some person on stackoverflow
      foreach (NetworkInterface nic in NetworkInterface
                   .GetAllNetworkInterfaces()) {
        /*
        log.Debug(
            "Found MAC Address: " + nic.GetPhysicalAddress() +
            " Type: " + nic.NetworkInterfaceType);
        */
        string tempMac = nic.GetPhysicalAddress().ToString();
        if (nic.Speed > maxSpeed && !string.IsNullOrEmpty(tempMac) &&
            tempMac.Length >= MIN_MAC_ADDR_LENGTH) {
          // log.Debug("New Max Speed = " + nic.Speed + ", MAC: " + tempMac);
          maxSpeed = nic.Speed;
          macAddress = tempMac;
        }
      }

    } catch (Exception exception) {
      Console.WriteLine("Unable to getMacAddress. " + exception.ToString());
    }

    return macAddress;
  }
}

/// <summary>
/// Provides simple time-related utility, primarily for getting the current time formatted as a string.
/// </summary>
public class Time {
  DateTime timeNow = DateTime.Now; // Stores the time when the object is instantiated.
  /// <summary>
  /// Defines the date and time pattern used for formatting: "M/d/yyyy hh:mm:ss tt".
  /// </summary>
  public static string datePatt = @"M/d/yyyy hh:mm:ss tt";

  /// <summary>
  /// Gets the current date and time, formatted according to the <see cref="datePatt"/> pattern.
  /// </summary>
  /// <returns>A string representation of the current date and time.</returns>
  public string getTimeNow() {
    DateTime dispDt = timeNow; // Uses the time captured when the Time object was instantiated
    string dtString;
    dtString = dispDt.ToString(datePatt);
    return dtString;
  }
}

/*
public void WriteToFile(string programCloseTime)
{
    time endingtime = new time();
    string path = @"D:\Google Drive\#zpersonal\LOGS\Antilobby\Log.txt";
    if (File.Exists(@"D:\Google Drive\#zpersonal\LOGS\Antilobby\Log.txt"))
    {
        //string content =
File.ReadAllText(@"C:\Users\Sean\Documents\ComputerLog\Log.txt");

        string newContent = "Session: " + MasterStartTime + " to " +
endingtime.getTimeNow() + " ||  Time Spend: " +
main.getConvertTime(main.TickCount) + Environment.NewLine;
        //label1.Text = newContent;
        //File.WriteAllText(path, newContent);
        File.AppendAllText(path, newContent);

    }
}
*/

}
