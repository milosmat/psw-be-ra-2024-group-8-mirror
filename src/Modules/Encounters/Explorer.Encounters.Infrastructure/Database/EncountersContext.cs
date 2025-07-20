using Explorer.Encounters.Core.Domain;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace Explorer.Encounters.Infrastructure.Database
{
    public class EncountersContext : DbContext
    {
        public DbSet<Encounter> Encounters { get; set; }
        public DbSet<TouristProfile> TouristProfiles { get; set; }

        public EncountersContext(DbContextOptions<EncountersContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("encounters");

            // Konfiguracija za TouristProfile entitet
            var valueComparer = new ValueComparer<List<long>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()
            );

            modelBuilder.Entity<TouristProfile>().ToTable("tourist_profiles")
                .Property(tp => tp.CompletedEncountersIds)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<long>>(v, (JsonSerializerOptions)null)
                )
                .Metadata.SetValueComparer(valueComparer);

            modelBuilder.Entity<TouristProfile>()
                .Property(tp => tp.XP)
                .HasDefaultValue(0);

            // Konfiguracija za Encounter entitet
            modelBuilder.Entity<Encounter>().ToTable("encounters")
                .Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (EncounterStatus)Enum.Parse(typeof(EncounterStatus), v)
                );

            modelBuilder.Entity<Encounter>().Property(e => e.Type)
                .HasConversion(
                    v => v.ToString(),
                    v => (EncounterType)Enum.Parse(typeof(EncounterType), v)
                );

            modelBuilder.Entity<Encounter>()
                .Property(e => e.PublishedDate)
                .HasColumnType("timestamp with time zone");

            modelBuilder.Entity<Encounter>()
                .Property(e => e.ArchivedDate)
                .HasColumnType("timestamp with time zone");

            modelBuilder.Entity<Encounter>()
                .OwnsOne(tp => tp.Location);

            base.OnModelCreating(modelBuilder);
        }

        public void SeedTouristProfiles(IEnumerable<User> tourists)
        {
            var existingTouristUsernames = TouristProfiles.Select(tp => tp.Username).ToHashSet();
            var newTourists = tourists
                .Where(t => !existingTouristUsernames.Contains(t.Username))
                .Select(u => new TouristProfile(u.Username, u.PasswordHash, u.Role, u.IsActive))
                .ToList();

            if (newTourists.Any())
            {
                TouristProfiles.AddRange(newTourists);
                SaveChanges();
            }
        }

        public void SeedData(IUserRepository userRepository)
        {
            // Dohvatanje svih turista
            var tourists = userRepository.GetUsersByRole(UserRole.Tourist);
            SeedTouristProfiles(tourists);
        }
    }
}
