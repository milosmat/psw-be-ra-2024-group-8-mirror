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
        modelBuilder.Entity<Blogg>().Property(item => item.Votes).HasColumnType("jsonb");
    }
}