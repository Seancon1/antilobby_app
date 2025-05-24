using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antilobby_2.Alert {
/*
 * Alerts class
 * Creates an object with information for an alert.
 *
 * */
class Alert {
  private string processName = null;
  private int alertLimit = 0;
  private int currentCount = 0;
  private string alertAction = "none";

  // Default constructor, must have both, default limit is 250 ticks
  /// <summary>
  /// Initializes a new instance of the <see cref="Alert"/> class with a process name and a default alert limit.
  /// </summary>
  /// <param name="inProcessName">The name of the process to monitor.</param>
  /// <param name="inAlertLimit">The tick limit for the alert. Defaults to 250.</param>
  public Alert(string inProcessName, int inAlertLimit = 250) {
    this.processName = inProcessName;
    this.alertLimit = inAlertLimit;
    this.currentCount = 0;
    this.AlertAction = "none";
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="Alert"/> class with a process name, alert limit, and action.
  /// </summary>
  /// <param name="inProcessName">The name of the process to monitor.</param>
  /// <param name="inAlertLimit">The tick limit for the alert. Defaults to 250.</param>
  /// <param name="AlertAction">The action to take when the alert limit is reached. Defaults to "none".</param>
  public Alert(string inProcessName, int inAlertLimit = 250,
               string AlertAction = "none") {
    this.processName = inProcessName;
    this.alertLimit = inAlertLimit;
    this.currentCount = 0;
    this.AlertAction = AlertAction;
  }

  /// <summary>
  /// Gets or sets the tick limit for the alert.
  /// </summary>
  public int AlertLimit {
    get => alertLimit;
    set => alertLimit = value;
  }
  /// <summary>
  /// Gets or sets the current tick count for the alert.
  /// </summary>
  public int CurrentCount {
    get => currentCount;
    set => currentCount = value;
  }
  /// <summary>
  /// Gets or sets the name of the process being monitored.
  /// </summary>
  public string ProcessName {
    get => processName;
    set => processName = value;
  }
  /// <summary>
  /// Gets or sets the action to take when the alert limit is reached.
  /// </summary>
  public string AlertAction {
    get => alertAction;
    set => alertAction = value;
  }

  /// <summary>
  /// Increments the current tick count for the alert.
  /// </summary>
  public void addTick() { currentCount++; }

  /// <summary>
  /// Resets the current tick count for the alert to zero.
  /// </summary>
  public void resetTick() { currentCount = 0; }

  /// <summary>
  /// Checks if the current tick count has reached or exceeded the alert limit.
  /// </summary>
  /// <returns>True if the limit is reached, false otherwise.</returns>
  public bool reachedLimit() {
    return (currentCount >= alertLimit) ? true : false;
  }
}
}
