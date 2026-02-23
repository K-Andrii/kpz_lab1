using System;
using System.Collections.Generic;

namespace VideoGamesCatalogApp.Models;

public partial class GameKey
{
    public int KeyId { get; set; }

    public string LicenseKey { get; set; } = null!;

    public DateTime? PurchaseDate { get; set; }

    public bool IsActive { get; set; }

    public int GameId { get; set; }

    public int StorefrontId { get; set; }

    public int? CurrentUserId { get; set; }

    public int? OrderId { get; set; }

    public string? RevocationReason { get; set; }

    public virtual User? CurrentUser { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual ICollection<GameSale> GameSales { get; set; } = new List<GameSale>();

    public virtual Order? Order { get; set; }

    public virtual Storefront Storefront { get; set; } = null!;
}
