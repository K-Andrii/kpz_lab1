using System;
using System.Collections.Generic;

namespace VideoGamesCatalogApp.Models;

public partial class Storefront
{
    public int StorefrontId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<GameKey> GameKeys { get; set; } = new List<GameKey>();
}
