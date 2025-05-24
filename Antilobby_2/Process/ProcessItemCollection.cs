using AntiLobby_2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antilobby_2 {
/// <summary>
/// Represents a collection of <see cref="ProcessItem"/> objects.
/// This class is typically used for serialization or grouping of process items.
/// </summary>
class ProcessItemCollection {
  /// <summary>
  /// Gets or sets the list of <see cref="ProcessItem"/> objects in the collection.
  /// </summary>
  public List<ProcessItem> data { get; set; }
}
}
