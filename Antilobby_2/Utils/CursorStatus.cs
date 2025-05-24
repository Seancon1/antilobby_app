using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antilobby_2.Utils {
/// <summary>
/// Tracks the status of the mouse cursor to determine if it's idle.
/// </summary>
class CursorStatus {
  int x = 0;
  int y = 0;
  int idleCount = 0;

  /// <summary>
  /// Initializes a new instance of the <see cref="CursorStatus"/> class.
  /// </summary>
  public CursorStatus() {}

  /// <summary>
  /// Compares the current cursor coordinates with its previous coordinates.
  /// If the coordinates are the same, an internal idle counter is incremented.
  /// If they are different, the idle counter is reset, and the new coordinates are stored.
  /// </summary>
  /// <param name="x">The current X coordinate of the cursor.</param>
  /// <param name="y">The current Y coordinate of the cursor.</param>
  /// <returns>True if the cursor position is the same as the previously recorded position (indicating idleness for this tick); false otherwise.</returns>
  public bool isCursorIdle(int x, int y) {

    if (this.x == x && this.y == y) {
      idleCount++;
      return true;
    } else {
      this.x = x;
      this.y = y;
      idleCount = 0;
    }

    return false;
  }

  /// <summary>
  /// Gets the current idle count. The count increments each time <see cref="isCursorIdle"/> is called
  /// and the cursor position has not changed. It resets when the cursor moves.
  /// </summary>
  /// <returns>The current idle tick count.</returns>
  public int getIdleCount() { return this.idleCount; }
}
}
