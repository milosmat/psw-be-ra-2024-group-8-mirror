using Explorer.Stakeholders.Core.Domain;
using Microsoft.EntityFrameworkCore;
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
    public DbSet<Followers> Followerss { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Notification> Notifications { get; set; }



    public StakeholdersContext(DbContextOptions<StakeholdersContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("stakeholders");
        //modelBuilder.HasDefaultSchema("appRatings");

        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<Club>().ToTable("Clubs");


        ConfigureStakeholder(modelBuilder);

        modelBuilder.Entity<Problem>().ToTable("Problems");
        modelBuilder.Entity<Followers>().ToTable("Followerss");
        modelBuilder.Entity<Message>().ToTable("Messages");
        modelBuilder.Entity<Notification>().ToTable("Notifications");

        modelBuilder.Entity<Notification>()
            .HasOne(n => n.Message)    
            .WithMany()                
            .HasForeignKey(n => n.MessageId) 
            .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<Notification>()
            .HasIndex(n => n.FollowerId);  

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
