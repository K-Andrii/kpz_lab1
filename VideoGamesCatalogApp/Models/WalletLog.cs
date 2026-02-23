using System;
using System.Collections.Generic;

namespace VideoGamesCatalogApp.Models;

public partial class WalletLog
{
    public int LogId { get; set; }

    public int UserId { get; set; }

    public decimal? OldBalance { get; set; }

    public decimal? NewBalance { get; set; }

    public DateTime ChangeDate { get; set; }
}
