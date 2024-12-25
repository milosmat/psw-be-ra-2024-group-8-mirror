using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces
{
    public interface IArticleRepository : ICrudRepository<Article>
    {
        List<Article> GetAll();

    }
}
