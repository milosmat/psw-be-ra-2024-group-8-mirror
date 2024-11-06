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
        /*
        public Blogg Update(Blogg aggregateRoot)
        {
            DbContext.Entry(aggregateRoot).State = EntityState.Modified;
            DbContext.Blogs.Update(aggregateRoot);
            DbContext.SaveChanges();
            return aggregateRoot;
        }*/

        public Blogg Update(Blogg aggregateRoot)
        {
            // Dodavanje novih komentara
            var newComments = aggregateRoot.Comments.Where(c => c.Id == 0).ToList();
            if (newComments.Any())
            {
                DbContext.Comments.AddRange(newComments);
            }

            // Ažuriranje postojećih komentara
            var existingComments = aggregateRoot.Comments.Where(c => c.Id != 0).ToList();
            if (existingComments.Any())
            {
                DbContext.Comments.UpdateRange(existingComments);
            }

            // Brisanje komentara koji više ne postoje u kolekciji
            var existingCommentIds = aggregateRoot.Comments.Select(c => c.Id).ToList();
            var commentsToDelete = DbContext.Comments
                .Where(c => c.BlogId == aggregateRoot.Id && !existingCommentIds.Contains(c.Id))
                .ToList();


            if (commentsToDelete.Any())
            {
                DbContext.Comments.RemoveRange(commentsToDelete);
            }

            
            DbContext.Entry(aggregateRoot).State = EntityState.Modified;
            DbContext.Blogs.Update(aggregateRoot);

            DbContext.SaveChanges();

            return aggregateRoot;
        }



    }
}
