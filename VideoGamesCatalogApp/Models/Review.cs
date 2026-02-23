using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VideoGamesCatalogApp.Models;

public partial class Review
{
    [Key]
    public int ReviewId { get; set; }

    public byte Rating { get; set; }

    public string? ReviewText { get; set; }

    public DateTime ReviewDate { get; set; }

    public bool IsApproved { get; set; }

    public bool IsEdited { get; set; }

    public int UserId { get; set; }

    public int GameId { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
