using System;
using System.Collections.Generic;

namespace VideoGamesCatalogApp.Models;

public partial class GameSale
{
    public int KeyId { get; set; }

    public int OrderId { get; set; }

    public decimal PricePaid { get; set; }

    public decimal? DiscountApplied { get; set; }

    public decimal? FinalPrice { get; set; }

    public virtual GameKey Key { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
