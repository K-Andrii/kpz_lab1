using System;
using System.Collections.Generic;

namespace VideoGamesCatalogApp.Models;

public partial class HardwareType
{
    public int TypeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<HardwareComponent> HardwareComponents { get; set; } = new List<HardwareComponent>();
}
