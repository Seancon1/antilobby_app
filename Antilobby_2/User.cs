using AntiLobby_2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antilobby_2 {
// Holds user identifying information

/// <summary>
/// Represents a user, storing identifying information such as IP address, MAC address, authentication token, and email.
/// </summary>
public class User {

  private string IP;
  private string MAC;
  private string userToken;
  private string email;

  /// <summary>
  /// Initializes a new instance of the <see cref="User"/> class.
  /// Automatically retrieves the local IP and MAC addresses. Email is initialized to "null".
  /// </summary>
  public User() {
    this.IP = "" + MyUtils.GetIPAddress();
    this.MAC = "" + MyUtils.getMacAddress();
    this.email = "null";
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="User"/> class with the specified IP and MAC addresses.
  /// Email is initialized to "null".
  /// </summary>
  /// <param name="inIP">The IP address of the user.</param>
  /// <param name="inMAC">The MAC address of the user.</param>
  public User(string inIP, string inMAC) {
    this.IP = inIP;
    this.MAC = inMAC;
    this.email = "null";
  }

  /// <summary>
  /// Gets or sets the IP address of the user.
  /// </summary>
  public string IP1 {
    get => IP;
    set => IP = value;
  }
  /// <summary>
  /// Gets or sets the MAC address of the user.
  /// </summary>
  public string MAC1 {
    get => MAC;
    set => MAC = value;
  }
  /// <summary>
  /// Gets or sets the authentication token for the user.
  /// </summary>
  public string Token {
    get => userToken;
    set => userToken = value;
  }
  /// <summary>
  /// Gets or sets the email address of the user.
  /// </summary>
  public string Email {
    get => email;
    set => email = value;
  }
}
}
