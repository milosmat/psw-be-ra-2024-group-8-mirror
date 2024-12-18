using Explorer.Games.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Explorer.Games.Infrastructure.Database
{
        public class GamesContext : DbContext
        {
            // Definicije DbSet entiteta
            public DbSet<Game> Games { get; set; }
            public DbSet<GameScore> GameScores { get; set; }

            // Konstruktor sa opcijama za konfiguraciju
            public GamesContext(DbContextOptions<GamesContext> options) : base(options) { }

            // OnModelCreating metod za podešavanje mapa
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.HasDefaultSchema("games");  // Setovanje šeme u bazi podataka

                // Mapa za Game entitet
                modelBuilder.Entity<Game>().ToTable("Games")
                    .HasMany(g => g.Scores)
                    .WithOne()  // Ako `GameScore` ne sadrži navigaciju natrag ka `Game`, koristimo `.WithOne()`
                    .OnDelete(DeleteBehavior.Cascade);  // Kada se Game obriše, brišu se i svi povezani GameScore objekti

                // Mapa za GameScore entitet
                modelBuilder.Entity<GameScore>().ToTable("GameScores");

                // Definisanje mogućnosti za JSON konverziju (ako je potrebno za kompleksnije tipove)
                modelBuilder.Entity<Game>()
                    .Property(g => g.Highscore)
                    .HasDefaultValue(0.0);  // Postavljanje podrazumevane vrednosti za Highscore

                // Ako imate neki složeniji tip koji želite konvertovati u JSON, kao što je lista rezultata
                // Ovo je primer ako biste želeli da `Game` entitet sadrži neku kolekciju u JSON formatu:
                modelBuilder.Entity<Game>()
                    .Property(item => item.Scores)  // Ovo zavisi od toga kako ćete pohraniti listu rezultata
                    .HasColumnType("jsonb")  // Ovaj tip koristi JSONB za PostgreSQL
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<GameScore>>(v, (JsonSerializerOptions)null)
                    );

                // Možete dodati i druge konverzije i konfiguracije u zavisnosti od složenosti domena

                base.OnModelCreating(modelBuilder);
            }


        }
}
