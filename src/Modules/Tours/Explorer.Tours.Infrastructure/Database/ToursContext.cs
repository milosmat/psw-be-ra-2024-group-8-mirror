using Explorer.Tours.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Object = Explorer.Tours.Core.Domain.Object;

namespace Explorer.Tours.Infrastructure.Database;

public class ToursContext : DbContext
{
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<TourPreferences> TourPreferences { get; set; }
    public DbSet<Tour> Tours { get; set; }
    public DbSet<TourCheckpoint> TourCheckpoints { get; set; }

    public DbSet<TouristEquipment> TouristEquipments { get; set; }

    public DbSet<TourReview> TourReviews { get; set; }

    public DbSet<Object> Objects { get; set; }

    public ToursContext(DbContextOptions<ToursContext> options) : base(options) { }

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


    }
}