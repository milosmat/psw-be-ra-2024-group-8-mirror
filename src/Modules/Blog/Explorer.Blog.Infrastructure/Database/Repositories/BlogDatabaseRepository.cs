using Explorer.Blog.Core.Domain.Blogs;
using Explorer.Blog.Core.Domain.RepositoryInterfaces;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Explorer.Blog.Infrastructure.Database.Repositories
{
    public class BlogDatabaseRepository : CrudDatabaseRepository<Blogg, BlogContext>, IBlogRepository
    {
        public BlogDatabaseRepository(BlogContext dbContext) : base(dbContext) { }

        public new Blogg? Get(long id)
        {
            return DbContext.Blogs.Where(t => t.Id == id)
                .Include(t => t.Comments!).FirstOrDefault();
        }
        public Blogg Update(Blogg aggregateRoot)
        {
            DbContext.Entry(aggregateRoot).State = EntityState.Modified;
            DbContext.SaveChanges();
            return aggregateRoot;
        }

    }
}
