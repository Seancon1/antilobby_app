using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Antilobby_2.Alert {

class AlertAction {
  Alert alert;
  Process process;
  Process[] processList;

  // static extern bool SetForegroundWindow(IntPtr hWnd);

  /// <summary>
  /// Initializes a new instance of the <see cref="AlertAction"/> class.
  /// </summary>
  public AlertAction() {}

  /// <summary>
  /// Initializes a new instance of the <see cref="AlertAction"/> class with the specified alert.
  /// </summary>
  /// <param name="alert">The alert configuration to use for actions.</param>
  public AlertAction(Alert alert) { this.alert = alert; }

  /// <summary>
  /// Attempts to close all running processes with the name specified in the associated alert configuration.
  /// </summary>
  public void closeProcess() {
    try {
      this.processList = Process.GetProcessesByName(alert.ProcessName);
      foreach (Process item in this.processList) {
        // process.Close();
        // process.SafeHandle.Close();
        item.Kill();
      }

    } catch (Exception e) {
      Debug.WriteLine(e.ToString());
    }
  }
  /// <summary>
  /// Sets the foreground window. (P/Invoke)
  /// </summary>
  /// <param name="hWnd">A handle to the window to bring to the foreground.</param>
  /// <returns>True if the function succeeds, false otherwise.</returns>
  [DllImport("USER32.DLL")]
  public static extern bool SetForegroundWindow(IntPtr hWnd);

  /// <summary>
  /// Shows or hides the specified window. (P/Invoke)
  /// </summary>
  /// <param name="handle">A handle to the window.</param>
  /// <param name="nCmdShow">Controls how the window is to be shown.</param>
  /// <returns>If the window was previously visible, the return value is nonzero. If the window was previously hidden, the return value is zero.</returns>
  [System.Runtime.InteropServices.DllImport("User32.dll")]
  public static extern bool ShowWindow(IntPtr handle, int nCmdShow);

  /*
   * Experimental Feature
   *
   */
  /// <summary>
  /// Attempts to bring all running processes with the name specified in the associated alert configuration to the foreground.
  /// This is an experimental feature.
  /// </summary>
  public void bringToFront() {

    try {
      /**
       * https://stackoverflow.com/questions/10898560/how-to-set-focus-to-another-window
       * there are various values of nCmdShow 3, 5 ,9. What 9 does is:
       *Activates and displays the window. If the window is minimized or
       *maximized, *the system restores it to its original size and position.An
       *application *should specify this flag when restoring a minimized window
       * https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-showwindow
       */
      /*
      this.process = Process.GetProcessesByName(alert.ProcessName)[0];
      Debug.WriteLine("Process Selected to Focus: " + process.ProcessName);
      var processHandle = this.process.MainWindowHandle;
      if(processHandle != null)
      {

          //SetForegroundWindow(processHandle.ToPointer());
          ShowWindow(processHandle, 1);
          SetForegroundWindow(processHandle);

      }
      */

      this.processList = Process.GetProcessesByName(alert.ProcessName);
      foreach (Process item in this.processList) {
        var processHandle = item.MainWindowHandle;
        if (processHandle != null) {

          ShowWindow(processHandle, 9);
          SetForegroundWindow(processHandle);
        }
      }

    } catch (Exception e) {
      Debug.WriteLine(e.ToString());
    }
  }
}
}
