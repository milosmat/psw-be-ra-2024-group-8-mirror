using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.TourProblems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace Explorer.Stakeholders.Infrastructure.Database;

public class StakeholdersContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Club> Clubs { get; set; }
    public DbSet<AppRating> AppRatings { get; set; }
    public DbSet<Problem> Problems { get; set; }
    public DbSet<TourProblem> TourProblems { get; set; }
    public DbSet<ProblemComment> ProblemComments { get; set; }



    public StakeholdersContext(DbContextOptions<StakeholdersContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("stakeholders");
        //modelBuilder.HasDefaultSchema("appRatings");

        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<Club>().ToTable("Clubs");


        ConfigureStakeholder(modelBuilder);

        modelBuilder.Entity<Problem>().ToTable("Problems");

        modelBuilder.Entity<TourProblem>().ToTable("TourProblems");
        modelBuilder.Entity<ProblemComment>().ToTable("ProblemComments");

        modelBuilder.Entity<TourProblem>()
            .HasMany(sc => sc.ProblemComments)  // Povezivanje sa kolekcijom stavki
            .WithOne() // Svaka stavka pripada jednom shopping cart-u
            .HasForeignKey("TourProblemId")  // Spoljni ključ
            .OnDelete(DeleteBehavior.Cascade);

    }
    
    private static void ConfigureStakeholder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasOne<User>()
            .WithOne()
            .HasForeignKey<Person>(s => s.UserId);
    }
    /*Add-Migration -Name Init -Context StakeholdersContext -Project Explorer.Stakeholders.Infrastructure -StartupProject Explorer.API
Update-Database -Context StakeholdersContext -Project Explorer.Stakeholders.Infrastructure -StartupProject Explorer.API
*/
}
