using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VideoGamesCatalogApp.Models;

public partial class VideoGamesCatalogContext : DbContext
{
    public VideoGamesCatalogContext()
    {
    }

    public VideoGamesCatalogContext(DbContextOptions<VideoGamesCatalogContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Developer> Developers { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameKey> GameKeys { get; set; }

    public virtual DbSet<GameSale> GameSales { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<HardwareComponent> HardwareComponents { get; set; }

    public virtual DbSet<HardwareType> HardwareTypes { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Platform> Platforms { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Storefront> Storefronts { get; set; }

    public virtual DbSet<SystemRequirement> SystemRequirements { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VwGameInfo> VwGameInfos { get; set; }

    public virtual DbSet<WalletLog> WalletLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=192.168.0.121;Initial Catalog=VideoGamesCatalog;User ID=sa;Password=123456789;TrustServerCertificate=True;Encrypt=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Developer>(entity =>
        {
            entity.HasKey(e => e.DeveloperId).HasName("PK__Develope__DE084CD15F2108A0");

            entity.HasIndex(e => e.Name, "ix_Developers_Name");

            entity.Property(e => e.DeveloperId).HasColumnName("DeveloperID");
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.GameId).HasName("PK__Games__2AB897DD9BA12D8A");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("trg_Games_UpdateAudit");
                    tb.HasTrigger("trg_ValidateGamePrice");
                });

            entity.HasIndex(e => e.Title, "ix_Games_Title");

            entity.HasIndex(e => new { e.Title, e.CurrentPrice }, "ix_Games_Title_Price");

            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.AgeRating).HasMaxLength(10);
            entity.Property(e => e.BasePrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CurrentPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DeveloperId).HasColumnName("DeveloperID");
            entity.Property(e => e.PublisherId).HasColumnName("PublisherID");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.TrailerUrl)
                .HasMaxLength(255)
                .HasColumnName("TrailerURL");

            entity.HasOne(d => d.Developer).WithMany(p => p.Games)
                .HasForeignKey(d => d.DeveloperId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Games__Developer__45F365D3");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Games)
                .HasForeignKey(d => d.PublisherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Games__Publisher__46E78A0C");

            entity.HasMany(d => d.Genres).WithMany(p => p.Games)
                .UsingEntity<Dictionary<string, object>>(
                    "GameGenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__GameGenre__Genre__778AC167"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__GameGenre__GameI__76969D2E"),
                    j =>
                    {
                        j.HasKey("GameId", "GenreId").HasName("PK__GameGenr__DA80C7886B949D88");
                        j.ToTable("GameGenres");
                        j.IndexerProperty<int>("GameId").HasColumnName("GameID");
                        j.IndexerProperty<int>("GenreId").HasColumnName("GenreID");
                    });

            entity.HasMany(d => d.Platforms).WithMany(p => p.Games)
                .UsingEntity<Dictionary<string, object>>(
                    "GamePlatform",
                    r => r.HasOne<Platform>().WithMany()
                        .HasForeignKey("PlatformId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__GamePlatf__Platf__7B5B524B"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__GamePlatf__GameI__7A672E12"),
                    j =>
                    {
                        j.HasKey("GameId", "PlatformId").HasName("PK__GamePlat__95ED08B0695DCC41");
                        j.ToTable("GamePlatforms");
                        j.IndexerProperty<int>("GameId").HasColumnName("GameID");
                        j.IndexerProperty<int>("PlatformId").HasColumnName("PlatformID");
                    });
        });

        modelBuilder.Entity<GameKey>(entity =>
        {
            entity.HasKey(e => e.KeyId).HasName("PK__GameKeys__21F5BE27CEEEBFE9");

            entity.HasIndex(e => e.LicenseKey, "UQ__GameKeys__45E1DD6F17EB5DB4").IsUnique();

            entity.Property(e => e.KeyId).HasColumnName("KeyID");
            entity.Property(e => e.CurrentUserId).HasColumnName("CurrentUserID");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LicenseKey)
                .HasMaxLength(36)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.RevocationReason).HasMaxLength(255);
            entity.Property(e => e.StorefrontId).HasColumnName("StorefrontID");

            entity.HasOne(d => d.CurrentUser).WithMany(p => p.GameKeys)
                .HasForeignKey(d => d.CurrentUserId)
                .HasConstraintName("FK__GameKeys__Curren__6D0D32F4");

            entity.HasOne(d => d.Game).WithMany(p => p.GameKeys)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GameKeys__GameID__6B24EA82");

            entity.HasOne(d => d.Order).WithMany(p => p.GameKeys)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__GameKeys__OrderI__6E01572D");

            entity.HasOne(d => d.Storefront).WithMany(p => p.GameKeys)
                .HasForeignKey(d => d.StorefrontId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GameKeys__Storef__6C190EBB");
        });

        modelBuilder.Entity<GameSale>(entity =>
        {
            entity.HasKey(e => new { e.KeyId, e.OrderId }).HasName("PK__GameSale__CDCCBB9DE4598895");

            entity.ToTable(tb => tb.HasTrigger("trg_AutoActivateKeyOnSale"));

            entity.Property(e => e.KeyId).HasColumnName("KeyID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.DiscountApplied).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.FinalPrice)
                .HasComputedColumnSql("([PricePaid]-isnull([DiscountApplied],(0)))", false)
                .HasColumnType("decimal(11, 2)");
            entity.Property(e => e.PricePaid).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Key).WithMany(p => p.GameSales)
                .HasForeignKey(d => d.KeyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GameSales__KeyID__7E37BEF6");

            entity.HasOne(d => d.Order).WithMany(p => p.GameSales)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GameSales__Order__7F2BE32F");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PK__Genres__0385055E0EB1F3A7");

            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<HardwareComponent>(entity =>
        {
            entity.HasKey(e => e.ComponentId).HasName("PK__Hardware__D79CF02E7F55897B");

            entity.Property(e => e.ComponentId).HasColumnName("ComponentID");
            entity.Property(e => e.Manufacturer).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.TypeId).HasColumnName("TypeID");

            entity.HasOne(d => d.Type).WithMany(p => p.HardwareComponents)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HardwareC__TypeI__66603565");
        });

        modelBuilder.Entity<HardwareType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__Hardware__516F039580604EDF");

            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAF74BD58C3");

            entity.HasIndex(e => e.OrderDate, "ix_OrderDate").HasFillFactor(70);

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.OrderDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.OrderStatus).HasMaxLength(50);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TransactionId)
                .HasMaxLength(255)
                .HasColumnName("TransactionID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__UserID__619B8048");
        });

        modelBuilder.Entity<Platform>(entity =>
        {
            entity.HasKey(e => e.PlatformId).HasName("PK__Platform__F559F6DAF48E472E");

            entity.Property(e => e.PlatformId).HasColumnName("PlatformID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.PublisherId).HasName("PK__Publishe__4C657E4BECC7BD81");

            entity.Property(e => e.PublisherId).HasColumnName("PublisherID");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Website).HasMaxLength(255);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId);

            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.IsApproved).HasDefaultValue(true);
            entity.Property(e => e.ReviewDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ReviewId)
                .ValueGeneratedOnAdd()
                .HasColumnName("ReviewID");
            entity.Property(e => e.ReviewText).HasMaxLength(2000);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Game).WithMany()
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__GameID__5AEE82B9");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__UserID__59FA5E80");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3AEAE5A076");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Storefront>(entity =>
        {
            entity.HasKey(e => e.StorefrontId).HasName("PK__Storefro__2F9958E4DF1A03BD");

            entity.Property(e => e.StorefrontId).HasColumnName("StorefrontID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<SystemRequirement>(entity =>
        {
            entity.HasKey(e => e.SysReqId).HasName("PK__SystemRe__68838F291D4783C4");

            entity.Property(e => e.SysReqId).HasColumnName("SysReqID");
            entity.Property(e => e.CpuId).HasColumnName("CPU_ID");
            entity.Property(e => e.DirectXversion)
                .HasMaxLength(10)
                .HasColumnName("DirectXVersion");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.GpuId).HasColumnName("GPU_ID");
            entity.Property(e => e.Os)
                .HasMaxLength(100)
                .HasColumnName("OS");
            entity.Property(e => e.RamGb).HasColumnName("Ram_GB");
            entity.Property(e => e.RequirementType).HasMaxLength(50);
            entity.Property(e => e.StorageGb).HasColumnName("Storage_GB");

            entity.HasOne(d => d.Cpu).WithMany(p => p.SystemRequirementCpus)
                .HasForeignKey(d => d.CpuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SystemReq__CPU_I__71D1E811");

            entity.HasOne(d => d.Game).WithMany(p => p.SystemRequirements)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SystemReq__GameI__70DDC3D8");

            entity.HasOne(d => d.Gpu).WithMany(p => p.SystemRequirementGpus)
                .HasForeignKey(d => d.GpuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SystemReq__GPU_I__72C60C4A");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC8B8920CD");

            entity.ToTable(tb => tb.HasTrigger("trg_AuditWalletChange"));

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4BC9FE293").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534A77536A6").IsUnique();

            entity.HasIndex(e => new { e.FirstName, e.LastName }, "ix_Users_FirstLast");

            entity.HasIndex(e => new { e.FirstName, e.LastName }, "ix_Users_FirstName_LastName");

            entity.HasIndex(e => new { e.LastName, e.FirstName }, "ix_Users_LastFirst");

            entity.HasIndex(e => new { e.LastName, e.FirstName }, "ix_Users_LastName_FirstName");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.BanReason).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(64)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.RegistrationDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Username).HasMaxLength(50);
            entity.Property(e => e.WalletBalance)
                .HasDefaultValueSql("((0.00))")
                .HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__RoleID__5441852A");
        });

        modelBuilder.Entity<VwGameInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_GameInfo");

            entity.Property(e => e.CurrentPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Developer).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<WalletLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Wallet_L__5E5499A8F58527D0");

            entity.ToTable("Wallet_Log");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.ChangeDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.NewBalance).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.OldBalance).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
