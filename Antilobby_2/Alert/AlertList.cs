using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antilobby_2.Alert {
class AlertList {

  private AlertPlay alertplay;
  private List<Alert> list;
  private List<Alert> disposeList;
  private AntiLobby_2.ProcessList processList;
  private AlertAction alertAction = new AlertAction();

  /// <summary>
  /// Initializes a new instance of the <see cref="AlertList"/> class.
  /// </summary>
  /// <param name="processList">The process list to monitor for alerts.</param>
  public AlertList(AntiLobby_2.ProcessList processList) {
    this.list = new List<Alert>();
    this.disposeList = new List<Alert>();
    this.processList =
        processList; // must be able to access the current processList
  }

  /**
   * String exemptProcess - the process that will be exempt when incrementing
   * all process ticks
   * */
  /// <summary>
  /// Increments the tick count for all alerts in the list.
  /// If an alert's action is "close", its tick is always incremented.
  /// Otherwise, if an alert's process name matches the exemptProcess, its tick is reset; otherwise, it's incremented.
  /// </summary>
  /// <param name="exemptProcess">The name of the process that should not have its non-"close" action alert ticks incremented (its tick will be reset instead).</param>
  public void incrementAllTicks(String exemptProcess) {
    foreach (Alert alert in this.list) {
      /*
       * exempt process is the current active process, so don't increment the
       * AFK timer. This AFK alert is for continuous seconds NOT ACTIVELY on, so
       * a reset is required
       */
      if (alert.AlertAction == "close") {
        alert.addTick();
      } else {
        if (alert.ProcessName == exemptProcess) {
          alert.resetTick();
        } else {
          alert.addTick();
        }
      }
    }
  }

  /*
   * Returns (a list) of all alerts inside the this.list with a tick count that
   * exceeds their limit'
   * */
  /// <summary>
  /// Identifies and processes alerts that have reached their tick limit.
  /// Performs actions based on the alert type (e.g., close process, bring to front).
  /// Alerts that are processed and marked for disposal are removed from the main list.
  /// Plays a sound if there are active alerts.
  /// </summary>
  /// <returns>A list of alerts that have become active, or null if no alerts are active.</returns>
  public List<Alert> ActiveAlerts() {
    AlertPlay alertSound = new AlertPlay();
    List<Alert> activeAlerts = new List<Alert>();

    foreach (Alert alert in this.list) {
      if (alert.AlertLimit < fetchTick(alert.ProcessName) && alert != null) {
        activeAlerts.Add(alert); // adds the alert to temp list of activeAlerts

        // Check the specific action for the alert when it becomes active
        AlertAction alertAction =
            new AlertAction(alert); // pass to AlertAction class
        switch (alert.AlertAction) {
        case "close":
          alertAction.closeProcess();
          this.disposeList.Add(alert);
          break;

        case "front":
        case "bring to focus":
        case "focus":
          alertAction.bringToFront();
          // this.disposeList.Add(alert);
          break;

        case "none":
          break;
        default:
          break;
        }
      }
    }

    ClearDisposeListFromList();

    if (activeAlerts.Count < 1) {
      return null;
    } else {
      alertSound.play();
      return activeAlerts;
    } // return null if no alerts present
  }

  /// <summary>
  /// Identifies alerts that have not yet reached their tick limit.
  /// Alerts marked for disposal are removed from the main list.
  /// </summary>
  /// <returns>A list of alerts that are currently passive (not yet triggered), or null if no alerts are passive.</returns>
  public List<Alert> PassiveAlerts() {
    List<Alert> passiveAlerts = new List<Alert>();

    foreach (Alert alert in this.list) {
      if (alert.AlertLimit > fetchTick(alert.ProcessName) && alert != null) {
        passiveAlerts.Add(alert); // adds the alert to temp list of
                                  // passiveAlerts
      }
    }

    ClearDisposeListFromList();

    if (passiveAlerts.Count < 1) {
      return null;
    } else {
      return passiveAlerts;
    } // return null if no alerts present
  }

  /**
   * ClearDisposeListFromList()
   * Action:
   *  Removes the alerts located in disposeList from the active list
   * ETC info:
   * dispostList - a List<Alert> that contains alerts that are pending deletion
   *
   * */
  /// <summary>
  /// Removes alerts that are marked for disposal (present in the disposeList) from the main alert list.
  /// </summary>
  public void ClearDisposeListFromList() {
    if (list != null && disposeList != null)
      if (list.Count > 0)
        if (disposeList.Count > 0) {
          foreach (Alert alert in disposeList) {
            removeAlert(alert);
          }
        }
  }

  /*
  public Alert GetAlertTime(string name)
  {
      if()
  }
  */

  /// <summary>
  /// Adds a new alert to the list with the specified process name and alert time.
  /// </summary>
  /// <param name="processName">The name of the process for the alert.</param>
  /// <param name="alertTime">The tick limit for the alert.</param>
  /// <returns>True if the alert was added successfully, false otherwise.</returns>
  public bool addNewAlert(string processName, int alertTime) {
    // Add a new alert to the list
    try {
      this.list.Add(new Alert(processName, alertTime));
    } catch (Exception ee) {
      return false;
    }
    return true;
  }

  /// <summary>
  /// Adds a new alert to the list with the specified process name, alert time, and action.
  /// </summary>
  /// <param name="processName">The name of the process for the alert.</param>
  /// <param name="alertTime">The tick limit for the alert.</param>
  /// <param name="alertAction">The action to take when the alert is triggered.</param>
  /// <returns>True if the alert was added successfully, false otherwise.</returns>
  public bool addNewAlert(string processName, int alertTime,
                          string alertAction) {
    // Add a new alert to the list
    try {
      this.list.Add(new Alert(processName, alertTime, alertAction));
    } catch (Exception ee) {
      return false;
    }
    return true;
  }

  /// <summary>
  /// Removes a specific alert object from the list.
  /// </summary>
  /// <param name="alert">The alert object to remove.</param>
  /// <returns>True if the alert was removed successfully, false otherwise.</returns>
  public bool removeAlert(Alert alert) {
    // Add a new alert to the list
    try {
      this.list.Remove(alert);
    } catch (Exception ee) {
      return false;
    }
    return true;
  }

  /// <summary>
  /// Removes all alerts associated with the given process name from the list.
  /// </summary>
  /// <param name="inprocessName">The name of the process whose alerts are to be removed.</param>
  public void removeAlertByName(String inprocessName) {
    foreach (Alert alert in this.list) {
      if (alert.ProcessName == inprocessName) {
        this.list.Remove(alert); // remove
      }
    }
  }

  /// <summary>
  /// Removes all alerts from the list.
  /// </summary>
  public void clearAlerts() { list.Clear(); }

  /// <summary>
  /// Retrieves the configured alert time (tick limit) for a given process name.
  /// </summary>
  /// <param name="processName">The name of the process.</param>
  /// <returns>The alert time (tick limit) if an alert for the process is found; otherwise, -1.</returns>
  public int fetchAlertTime(string processName) {
    foreach (Alert alert in this.list) {
      if (alert.ProcessName == processName) {
        return alert.AlertLimit; // return if processName equals the same that
                                 // is being searched
      }
    }

    return -1;
  }

  /// <summary>
  /// Retrieves the current tick count for an alert associated with a given process name.
  /// </summary>
  /// <param name="processName">The name of the process.</param>
  /// <returns>The current tick count if an alert for the process is found; otherwise, -1.</returns>
  public int fetchTick(string processName) {
    foreach (Alert alert in this.list) {
      if (alert.ProcessName == processName) {
        return alert.CurrentCount;
      }
    }

    return -1;
  }

  /// <summary>
  /// Checks if the alert list is empty.
  /// </summary>
  /// <returns>True if the list contains no alerts, false otherwise.</returns>
  public bool isEmpty() {
    if (list.Count() > 0) {
      return false;
    } else {
      return true;
    }
  }
}
}
