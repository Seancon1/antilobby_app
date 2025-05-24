using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antilobby_2.Alert {
class AlertPlay {
  private Boolean active;

  // System.Media.SystemSounds.Hand.Play();

  /// <summary>
  /// Initializes a new instance of the <see cref="AlertPlay"/> class, with the alert sound initially turned off.
  /// </summary>
  public AlertPlay() { this.active = false; }

  /// <summary>
  /// Deactivates the alert sound.
  /// </summary>
  public void turnOff() { this.active = false; }

  /// <summary>
  /// Activates the alert sound. (Note: This method currently only sets a flag and doesn't continuously play sound).
  /// </summary>
  public void turnOn() { this.active = true; }

  /// <summary>
  /// Plays the system's "Hand" sound once.
  /// </summary>
  public void play() { System.Media.SystemSounds.Hand.Play(); }
}
}
