using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Object = Explorer.Tours.Core.Domain.Object;

namespace Explorer.Tours.Infrastructure.Database;

public class ToursContext : DbContext
{
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<TourPreferences> TourPreferences { get; set; }
    public DbSet<Tour> Tours { get; set; }
    public DbSet<TourCheckpoint> TourCheckpoints { get; set; }
    public DbSet<TourExecution> TourExecutions { get; set; }
    public DbSet<TouristEquipment> TouristEquipments { get; set; }

    public DbSet<TourReview> TourReviews { get; set; }

    public DbSet<Object> Objects { get; set; }

    public DbSet<ShoppingCart> ShoppingCard { get; set; }

    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; } // Dodajemo ShoppingCartItem kao zaseban DbSet

    public DbSet<TourPurchaseToken> Tokens { get; set; }


    public DbSet<TouristPosition> TouristPositions { get; set; }
    public DbSet<VisitedCheckpoint> VisitedCheckpoints { get; set; }

    public ToursContext(DbContextOptions<ToursContext> options) : base(options) {}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("tours");
        modelBuilder.Entity<Object>()
        .Property(o => o.Category)
        .HasConversion(
        v => v.ToString(),
        v => (ObjectCategory)Enum.Parse(typeof(ObjectCategory), v)
    );


        modelBuilder.Entity<Equipment>().ToTable("Equipment");
        modelBuilder.Entity<Tour>().ToTable("Tours")
            .HasMany(t => t.TourReviews)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<TourCheckpoint>().ToTable("TourCheckpoints");
        modelBuilder.Entity<TourReview>().ToTable("TourReviews");
        //modelBuilder.Entity<Tour>().Property(item => item.TravelTimes).HasColumnType("jsonb");
        modelBuilder.Entity<TourCheckpoint>().ToTable("TourCheckpoint");
        modelBuilder.Entity<TouristEquipment>().ToTable("TouristEquipments");
        modelBuilder.Entity<TourExecution>().ToTable("TourExecution");

        modelBuilder.Entity<TouristPosition>().ToTable("TouristPositions");
        modelBuilder.Entity<VisitedCheckpoint>().ToTable("VisitedCheckpoints");

        modelBuilder.Entity<ShoppingCart>().ToTable("ShoppingCart");
        modelBuilder.Entity<ShoppingCartItem>().ToTable("ShoppingCartItems"); // Konfiguracija tabele za ShoppingCartItem
        modelBuilder.Entity<TourPurchaseToken>().ToTable("Tokens");

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


        modelBuilder.Entity<TouristPosition>()
        .OwnsOne(tp => tp.CurrentLocation);
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tour>()
        .HasMany(t => t.TourCheckpoints)    // Navodi se kolekcija TourCheckpoints unutar Tour
        .WithOne()                           // Navodi se da TourCheckpoint ima referencu na Tour (bez navigacione property)
        .HasForeignKey(tc => tc.TourId);     // TourId se koristi kao spoljni ključ u TourCheckpoint


    }
}