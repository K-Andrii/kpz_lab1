using System;
using System.Collections.Generic;

namespace VideoGamesCatalogApp.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Phone { get; set; }

    public decimal WalletBalance { get; set; }

    public DateTime RegistrationDate { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public bool IsBlocked { get; set; }

    public string? BanReason { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<GameKey> GameKeys { get; set; } = new List<GameKey>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role Role { get; set; } = null!;
}
