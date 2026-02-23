using System;
using System.Collections.Generic;

namespace VideoGamesCatalogApp.Models;

public partial class VwGameInfo
{
    public string Title { get; set; } = null!;

    public string Developer { get; set; } = null!;

    public decimal CurrentPrice { get; set; }
}
