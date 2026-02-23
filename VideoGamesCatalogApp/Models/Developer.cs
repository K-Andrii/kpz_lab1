using System;
using System.Collections.Generic;

namespace VideoGamesCatalogApp.Models;

public partial class Developer
{
    public int DeveloperId { get; set; }

    public string Name { get; set; } = null!;

    public string? Country { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
