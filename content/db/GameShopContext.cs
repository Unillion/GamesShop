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
    }
}