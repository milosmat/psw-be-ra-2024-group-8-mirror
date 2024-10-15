using Explorer.Tours.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Explorer.Tours.Infrastructure.Database;

public class ToursContext : DbContext
{
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<TourPreferences> TourPreferences { get; set; }  

    public ToursContext(DbContextOptions<ToursContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("tours");

        // Mapping for Dictionary<TransportMode, int>
        modelBuilder.Entity<TourPreferences>()
        .Property(p => p.TransportPreferences)
        .HasConversion(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null), 
            v => JsonSerializer.Deserialize<Dictionary<TransportMode, int>>(v, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) 
        );

        // Mapping for List<string>
        modelBuilder.Entity<TourPreferences>()
            .Property(p => p.InterestTags)
            .HasConversion(
                v => string.Join(',', v), 
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() 
            );
    }
}