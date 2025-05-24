using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Antilobby_2 {
public partial class LoginClient : Form {

  Session Session;
  String storedValue; // Stores a value based on web browser navigation, used to trigger actions like closing the form.

  // Constructor for the LoginClient form.
  internal LoginClient(Session session) {
    InitializeComponent();
    WebBrowser clientView = webBrowser; // Assigns the form's WebBrowser control to a local variable (not used further).
    this.Session = session; // Stores the passed Session object for use within this form.
  }

  // Called when the WebBrowser navigates; checks the URL for specific content to extract a token.
  private string UserNavigated() {
    txtUrlStatus.Text = "[" + webBrowser.Url + "]"; // Display the current URL in a status label.

    /*
     * Unsafe way to link a value to attach sessions, will implement a better
     * way later Action: When user url contains @, entire url contents will be
     * saved to userToken for PrestigeCode account linking
     * */
    // If the URL contains "@", it's assumed to be part of a token or special URL.
    // The full URL is saved as the user token and "client_close" is returned to signal form closure.
    if (webBrowser.Url.ToString().Contains("@")) {
      Session.saveUserToken(webBrowser.Url.ToString()); // Save the URL string as the user token.

      return "client_close"; // Signal to close the client.
    }

    return null; // No specific action required based on the URL.
  }

  // Event handler for the WebBrowser's Navigated event (after navigation is complete).
  private void webBrowser_Navigated(object sender,
                                    WebBrowserNavigatedEventArgs e) {
    string returnValue = storedValue; // Use the value captured during Navigating event.

    if (storedValue == null) {
      // nothing returned
    } else {
      switch (storedValue) {
      case "client_close":
        this.Close(); // Close the form if "client_close" was signaled.
        break;
      default:
        break;
      }
    }
  }

  // Event handler for the cancel label click. Closes the form.
  private void lblLoginClientCancel_Click(object sender, EventArgs e) {
    this.Close();
  }

  // Event handler for the URL status label click (likely for debugging or a manual override).
  private void txtUrlStatus_Click(object sender, EventArgs e) {
    global.isLoggedIn = true; // Manually set the global login status to true.
  }

  // Event handler for the WebBrowser's Navigating event (before navigation occurs).
  private void webBrowser_Navigating(object sender,
                                     WebBrowserNavigatingEventArgs e) {
    storedValue = UserNavigated(); // Check the URL being navigated to and store any relevant signal.
  }

  // Event handler for the register button click. Opens the registration page in the default system browser.
  private void btnLoginClientRegister_Click(object sender, EventArgs e) {
    // switchLoginRegister(); // This was commented out, would toggle UI view.
    try {
      System.Diagnostics.Process.Start(
          "https://antilobby.prestigecode.com/register"); // Open registration URL.
      this.Close(); // Close the login form after opening the link.
    } catch (Exception x) {
      MessageBox.Show("Unable to open the website." + e.ToString()); // Show error if opening URL fails.
    }
  }

  // Asynchronously performs the login operation and updates UI accordingly.
  private async Task<bool> performDoLoginAsync() {
    lblWorking.Visible = true; // Show "Working..." label.
    lblWorking.Text = "Working...";
    // Await the actual login logic and update label based on success or failure.
    lblWorking.Text = !await doLogin() ? "Unable to login" : "Success";
    return true; // Returns true regardless of login success, UI is updated by text.
  }

  // Event handler for the login button click.
  private async void btnLoginClientLogin_Click(object sender, EventArgs e) {
    await performDoLoginAsync(); // Initiate the asynchronous login process.
  }

  // Event handler for the "Return to Login" label click. Toggles UI view between login and web browser view.
  private void lblLoginClientReturnToLogin_Click(object sender, EventArgs e) {
    switchLoginRegister();
  }

  // Toggles the visibility of the WebBrowser and the main login panel (panLogin).
  private void switchLoginRegister() {
    webBrowser.Visible = !webBrowser.Visible; // Toggle WebBrowser visibility.
    lblLoginClientReturnToLogin.Visible = !lblLoginClientReturnToLogin.Visible; // Toggle "Return to Login" label visibility.
    panLogin.Visible = !panLogin.Visible; // Toggle login panel visibility.
  }

  // Performs the core login logic by sending credentials to the API.
  private async Task<bool> doLogin() {
    // putting a new reference to logger to prevent errors when Session is null
    AntiLobby_2.Logger logger = new AntiLobby_2.Logger(); // Local logger instance.
    var returnString = "";
    using (var client = new HttpClient()) {
      // Prepare login credentials from textboxes.
      var values = new Dictionary<string, string> {
        { "email", "" + txtEmail.Text },
        { "password", "" + txtPassword.Text },
        { "device_name", "antilobby_app" }, // Identifies the application making the request.
      };

      var content = new FormUrlEncodedContent(values);
      // Post credentials to the token endpoint.
      var response = await client.PostAsync(
          "https://antilobby.prestigecode.com/sanctum/token", content);
      var responseString = await response.Content.ReadAsStringAsync();

      // response modification, probably not the best way to detect an error
      // but I expect a string 100% of the time to be smaller than 256 chars
      // Process the response string to extract the token.
      returnString =
          (responseString.Length < 256) ? "" + responseString.ToString() : null; // Basic validation of response length.
      Session.setInMemoryUserToken(returnString); // Store token in session.
      Session.saveUserToken(returnString);         // Save token locally.
      // logger.SaveOfflineGeneric("_UserToken.antilobby", new string[] {
      // returnString });

      global.isLoggedIn = Session.hasInMemoryUserToken(); // Update global login status.
      Session.setInMemoryUserEmail(txtEmail.Text);        // Store email in session.

      // httpClient.DefaultRequestHeaders.Authorization = new
      // AuthenticationHeaderValue("Bearer", "Your Oauth token");
      if (returnString == null) { // If token is null (login failed).
        MessageBox.Show("There was an error processing your credentials.");
        global.needsRestart = false; // No restart needed on failed login.
        return false; // Indicate login failure.
      } else { // If token received (login successful).
        global.needsRestart = true; // Signal that main form might need to update/restart its state.
        return true; // Indicate login success.
      }
    }
  }

  // Timer tick event handler. Checks global login status and closes the form if logged in.
  private void timer1_Tick(object sender, EventArgs e) {
    if (global.isLoggedIn) { // If login is successful (flag set elsewhere, e.g., after doLogin).
      this.Close();           // Close the login form.
      MessageBox.Show("Logged in successful. The client is now authorized to " +
                      "save data on your behalf."); // Notify user.
    }
  }

  // Handles KeyDown event in the password textbox to allow login on Enter key press.
  private async void txtPassword_KeyDown(object sender, KeyEventArgs e) {
    if (e.KeyCode == Keys.Return) { // If Enter key is pressed.
      await performDoLoginAsync();    // Initiate the login process.
    }
  }
}
}
