using System;
using System.Collections.Generic;

namespace VideoGamesCatalogApp.Models;

public partial class Publisher
{
    public int PublisherId { get; set; }

    public string Name { get; set; } = null!;

    public string? Website { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
