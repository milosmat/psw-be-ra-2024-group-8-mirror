using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Domain;

namespace Explorer.Tours.Infrastructure.Database.Repositories
{
    public class ArticleDataBaseRepository : CrudDatabaseRepository<Article, ToursContext>, IArticleRepository
    {
        public ArticleDataBaseRepository(ToursContext dbContext) : base(dbContext)
        {
        }

        public List<Article> GetAll()
        {
            return DbContext.Set<Article>().ToList();
        }
    }
}
