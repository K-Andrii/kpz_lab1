using System;
using System.Collections.Generic;

namespace VideoGamesCatalogApp.Models;

public partial class SystemRequirement
{
    public int SysReqId { get; set; }

    public string RequirementType { get; set; } = null!;

    public byte RamGb { get; set; }

    public short StorageGb { get; set; }

    public string Os { get; set; } = null!;

    public string? DirectXversion { get; set; }

    public int GameId { get; set; }

    public int CpuId { get; set; }

    public int GpuId { get; set; }

    public virtual HardwareComponent Cpu { get; set; } = null!;

    public virtual Game Game { get; set; } = null!;

    public virtual HardwareComponent Gpu { get; set; } = null!;
}
