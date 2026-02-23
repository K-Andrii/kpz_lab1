using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VideoGamesCatalogApp.Models;

public partial class Game
{
    public int GameId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public string? AgeRating { get; set; }

    public decimal BasePrice { get; set; }

    public decimal CurrentPrice { get; set; }

    public byte? MetaScore { get; set; }

    public string? TrailerUrl { get; set; }

    public int DeveloperId { get; set; }

    public int PublisherId { get; set; }

    public virtual Developer Developer { get; set; } = null!;

    public virtual ICollection<GameKey> GameKeys { get; set; } = new List<GameKey>();

    public virtual Publisher Publisher { get; set; } = null!;

    public virtual ICollection<SystemRequirement> SystemRequirements { get; set; } = new List<SystemRequirement>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public virtual ICollection<Platform> Platforms { get; set; } = new List<Platform>();
}
