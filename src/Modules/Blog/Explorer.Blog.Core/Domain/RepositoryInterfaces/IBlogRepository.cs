using Explorer.Blog.Core.Domain.Blogs;
using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.Core.Domain.RepositoryInterfaces
{
    public interface IBlogRepository: ICrudRepository<Blogg>
    {
        PagedResult<Blogg> GetPaged(int page, int pageSize);
        Blogg Create(Blogg newBlog);
        void Delete(long id);

        Blogg Update(Blogg updateBlog);

        Blogg Get(int id);

    }
}
