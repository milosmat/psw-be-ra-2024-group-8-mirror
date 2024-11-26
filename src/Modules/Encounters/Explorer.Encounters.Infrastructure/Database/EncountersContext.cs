using Explorer.Encounters.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Explorer.Encounters.Infrastructure.Database
{
    public class EncountersContext : DbContext
    {
        public DbSet<Encounter> Encounters { get; set; }

        public EncountersContext(DbContextOptions<EncountersContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Postavljanje podrazumevanog šema za bazu
            modelBuilder.HasDefaultSchema("encounters");

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
                .HasColumnType("timestamp with time zone"); // PostgreSQL primer za vremenske zone

            modelBuilder.Entity<Encounter>()
                .Property(e => e.ArchivedDate)
                .HasColumnType("timestamp with time zone");

            modelBuilder.Entity<Encounter>()
                .OwnsOne(tp => tp.Location);
            base.OnModelCreating(modelBuilder);


            base.OnModelCreating(modelBuilder);
        }
    }
}
