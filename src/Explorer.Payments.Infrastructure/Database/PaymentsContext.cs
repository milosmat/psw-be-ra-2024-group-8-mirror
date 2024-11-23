using Explorer.Payments.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Infrastructure.Database;

public class PaymentsContext : DbContext
{
    public DbSet<ShoppingCart> ShoppingCard { get; set; }

    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

    public DbSet<TourPurchaseToken> Tokens { get; set; }

    public DbSet<Wallet> Wallets { get; set; }

    public DbSet<Transaction> Transactions { get; set; }

    public PaymentsContext(DbContextOptions<PaymentsContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("payments");

        modelBuilder.Entity<ShoppingCart>().ToTable("ShoppingCart");
        modelBuilder.Entity<ShoppingCartItem>().ToTable("ShoppingCartItems"); // Konfiguracija tabele za ShoppingCartItem
        modelBuilder.Entity<TourPurchaseToken>().ToTable("Tokens");
        modelBuilder.Entity<Wallet>().ToTable("Wallets");
        modelBuilder.Entity<Transaction>().ToTable("Transaction");

        modelBuilder.Entity<ShoppingCart>()
       .Property(sc => sc.ShopItemsCapacity)
       .HasColumnName("ShopingItems_Capacity")
       .IsRequired(false);  // Omogućava null vrednost za ovu kolonu

        // Povezivanje ShoppingCart i ShoppingCartItem
        modelBuilder.Entity<ShoppingCart>()
            .HasMany(sc => sc.ShopingItems)  // Povezivanje sa kolekcijom stavki
            .WithOne() // Svaka stavka pripada jednom shopping cart-u
            .HasForeignKey("ShoppingCartId")  // Spoljni ključ
            .OnDelete(DeleteBehavior.Cascade);  // Ako se obriše korpa, brišu se i stavke

    }
}
