using Explorer.Games.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Games.Infrastructure.Database
{
    public class GamesContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<GameScore> GameScores { get; set; }

        public GamesContext(DbContextOptions<GamesContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("games"); // Postavljanje šeme

            // Konfiguracija za Game entitet
            modelBuilder.Entity<Game>().ToTable("games")
                .Property(g => g.Highscore)
                .HasDefaultValue(0.0);

            // Konfiguracija za GameScore entitet
            modelBuilder.Entity<GameScore>().ToTable("game_scores")
                .HasKey(gs => gs.Id); // Primarni ključ za GameScore

            // Postavljanje veze između Game i GameScores
            modelBuilder.Entity<Game>()
                .HasMany(g => g.Scores)
                .WithOne() // GameScore nema navigaciju nazad ka Game
                .HasForeignKey("GameId") // Spoljni ključ u tabeli GameScores
                .OnDelete(DeleteBehavior.Cascade); // Kaskadno brisanje GameScore kada se Game obriše

            base.OnModelCreating(modelBuilder);
        }
    }
}
