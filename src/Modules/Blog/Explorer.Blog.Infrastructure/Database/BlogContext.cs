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

        modelBuilder.Entity<Comment>()
       .ToTable("Comments")
       .Property(c => c.Id)
       .ValueGeneratedOnAdd();  // Automatski generisan ID

        modelBuilder.Entity<Comment>()
            .HasOne<Blogg>() //komentar je vezan za jedan blog
            .WithMany(b => b.Comments) //blog ima vise komentara
            .HasForeignKey(c => c.BlogId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Comment>()
            .Property(c => c.CreationTime)
            .HasDefaultValueSql("timezone('utc', now())");


        modelBuilder.Entity<Blogg>().Property(item => item.Votes).HasColumnType("jsonb");
    }
}