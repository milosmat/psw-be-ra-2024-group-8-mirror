using Explorer.Blog.API.Dtos;
using Explorer.BuildingBlocks.Core.UseCases;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.API.Public
{
    public interface IBlogsService
    {
        //Result Vote(int id, Vote voteDto);
        PagedResult<Blogg> GetPaged(int page, int pageSize);
        Blogg Create(Blogg newBlog);
        void Delete(long id);
        Blogg Update(Blogg updateBlog);
        Blogg Get(long id);
    }
}
