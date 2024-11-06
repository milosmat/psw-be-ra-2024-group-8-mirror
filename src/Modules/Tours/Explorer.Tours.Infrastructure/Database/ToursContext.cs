using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
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
        modelBuilder.Entity<Tour>().ToTable("Tours");
        modelBuilder.Entity<TourCheckpoint>().ToTable("TourCheckpoints");
        //modelBuilder.Entity<Tour>().Property(item => item.TravelTimes).HasColumnType("jsonb");
        modelBuilder.Entity<TourCheckpoint>().ToTable("TourCheckpoint");
        modelBuilder.Entity<TouristEquipment>().ToTable("TouristEquipments");
        modelBuilder.Entity<TourExecution>().ToTable("TourExecution");

        modelBuilder.Entity<TouristPosition>().ToTable("TouristPositions");
        modelBuilder.Entity<VisitedCheckpoint>().ToTable("VisitedCheckpoints");

        modelBuilder.Entity<TouristPosition>()
        .OwnsOne(tp => tp.CurrentLocation);
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tour>()
        .HasMany(t => t.TourCheckpoints)    // Navodi se kolekcija TourCheckpoints unutar Tour
        .WithOne()                           // Navodi se da TourCheckpoint ima referencu na Tour (bez navigacione property)
        .HasForeignKey(tc => tc.TourId);     // TourId se koristi kao spoljni ključ u TourCheckpoint


    }
}