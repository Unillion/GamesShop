using GamesShop.content.models;
using Microsoft.EntityFrameworkCore;

public class GameShopContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Library> Libraries { get; set; }
    public DbSet<LibraryItem> LibraryItems { get; set; }
    public DbSet<Bill> Bills { get; set; }
    public DbSet<Balance> Balances { get; set; }
    public DbSet<Developers> Developers { get; set; }
    public DbSet<GameDevelopers> GameDevelopers { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<GameLanguage> GameLanguages { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<GameGenres> GameGenres { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=MISHA1\SQLEXPRESS01;Database=gameshopdb;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Cart>().ToTable("Carts");
        modelBuilder.Entity<Library>().ToTable("Libraries");
        modelBuilder.Entity<Bill>().ToTable("bill");
        modelBuilder.Entity<Balance>().ToTable("Balances");
        modelBuilder.Entity<Game>().ToTable("Games");
        modelBuilder.Entity<CartItem>().ToTable("CartItems");
        modelBuilder.Entity<LibraryItem>().ToTable("LibraryItems");
        modelBuilder.Entity<Developers>().ToTable("Developers");
        modelBuilder.Entity<GameDevelopers>().ToTable("GameDevelopers");
        modelBuilder.Entity<Language>().ToTable("Languages");
        modelBuilder.Entity<GameLanguage>().ToTable("GameLanguages");
        modelBuilder.Entity<Review>().ToTable("Reviews");
        modelBuilder.Entity<Genre>().ToTable("Genres");
        modelBuilder.Entity<GameGenres>().ToTable("GameGenres");

        modelBuilder.Entity<User>()
            .HasOne(u => u.Cart)
            .WithOne(c => c.User)
            .HasForeignKey<Cart>(c => c.UserID);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Library)
            .WithOne(l => l.User)
            .HasForeignKey<Library>(l => l.UserID);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Bill)
            .WithOne(b => b.User)
            .HasForeignKey<Bill>(b => b.UserID);

        modelBuilder.Entity<Bill>()
            .HasMany(b => b.Balances)
            .WithOne(bal => bal.Bill)
            .HasForeignKey(bal => bal.BillID);

        modelBuilder.Entity<Cart>()
            .HasMany(c => c.CartItems)
            .WithOne(ci => ci.Cart)
            .HasForeignKey(ci => ci.CartID);

        modelBuilder.Entity<Library>()
            .HasMany(l => l.LibraryItems)
            .WithOne(li => li.Library)
            .HasForeignKey(li => li.LibraryID);

        modelBuilder.Entity<Game>()
            .HasMany(g => g.CartItems)
            .WithOne(ci => ci.Game)
            .HasForeignKey(ci => ci.GameID);

        modelBuilder.Entity<Game>()
            .HasMany(g => g.LibraryItems)
            .WithOne(li => li.Game)
            .HasForeignKey(li => li.GameID);

        modelBuilder.Entity<GameDevelopers>()
            .HasOne(gd => gd.Game)
            .WithMany(g => g.GameDevelopers)
            .HasForeignKey(gd => gd.GameID);

        modelBuilder.Entity<GameDevelopers>()
            .HasOne(gd => gd.Developer)
            .WithMany(d => d.GameDevelopers)
            .HasForeignKey(gd => gd.DeveloperID);

        modelBuilder.Entity<GameDevelopers>()
            .HasIndex(gd => new { gd.GameID, gd.DeveloperID })
            .IsUnique();

        modelBuilder.Entity<GameLanguage>()
            .HasOne(gl => gl.Game)
            .WithMany(g => g.GameLanguages)
            .HasForeignKey(gl => gl.GameID);

        modelBuilder.Entity<GameLanguage>()
            .HasOne(gl => gl.Language)
            .WithMany(l => l.GameLanguages)
            .HasForeignKey(gl => gl.LanguageID);

        modelBuilder.Entity<GameLanguage>()
            .HasIndex(gl => new { gl.GameID, gl.LanguageID })
            .IsUnique();

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Game)
            .WithMany(g => g.Reviews)
            .HasForeignKey(r => r.GameID);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserID);

        modelBuilder.Entity<GameGenres>()
            .HasOne(gg => gg.Game)
            .WithMany(g => g.GameGenres)
            .HasForeignKey(gg => gg.GameID);

        modelBuilder.Entity<GameGenres>()
            .HasOne(gg => gg.Genre)
            .WithMany(g => g.GameGenres)
            .HasForeignKey(gg => gg.GenreID);

        // Уникальный ключ для GameGenres
        modelBuilder.Entity<GameGenres>()
            .HasIndex(gg => new { gg.GameID, gg.GenreID })
            .IsUnique();
    }
}