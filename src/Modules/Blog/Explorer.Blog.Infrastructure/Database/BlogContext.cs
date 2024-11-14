using Explorer.Blog.Core.Domain;
using Explorer.Blog.Core.Domain.Blogs;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Blog.Infrastructure.Database;

public class BlogContext : DbContext
{
    public DbSet<Blogg> Blogs { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public BlogContext(DbContextOptions<BlogContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("blog");
        modelBuilder.Entity<Comment>().ToTable("Comments");

        modelBuilder.Entity<Comment>()
       .ToTable("Comments")
       .Property(c => c.Id)
       .ValueGeneratedOnAdd();  // Automatski generisan ID

        modelBuilder.Entity<Blogg>()
        .HasMany(b => b.Comments)    // Navodi se kolekcija Comments unutar Bloga
        .WithOne()                           // Navodi se da Comment ima referencu na Blog (bez navigacione property)
        .HasForeignKey(c => c.BlogId);

        modelBuilder.Entity<Comment>()
            .Property(c => c.CreationTime)
            .HasDefaultValueSql("timezone('utc', now())");

        modelBuilder.Entity<Comment>()
       .HasOne(c => c.Blog)
       .WithMany(b => b.Comments)
       .HasForeignKey(c => c.BlogId)
       .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<Blogg>().Property(item => item.Votes).HasColumnType("jsonb");
    }
}