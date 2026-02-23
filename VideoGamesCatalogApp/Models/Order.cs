using System;
using System.Collections.Generic;

namespace VideoGamesCatalogApp.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string OrderStatus { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public string? TransactionId { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<GameKey> GameKeys { get; set; } = new List<GameKey>();

    public virtual ICollection<GameSale> GameSales { get; set; } = new List<GameSale>();

    public virtual User User { get; set; } = null!;
}
