using System;
using System.Collections.Generic;

namespace VideoGamesCatalogApp.Models;

public partial class HardwareComponent
{
    public int ComponentId { get; set; }

    public string Name { get; set; } = null!;

    public string? Manufacturer { get; set; }

    public int? BenchmarkScore { get; set; }

    public int TypeId { get; set; }

    public virtual ICollection<SystemRequirement> SystemRequirementCpus { get; set; } = new List<SystemRequirement>();

    public virtual ICollection<SystemRequirement> SystemRequirementGpus { get; set; } = new List<SystemRequirement>();

    public virtual HardwareType Type { get; set; } = null!;
}
