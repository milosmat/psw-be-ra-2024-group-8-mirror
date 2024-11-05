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
        PagedResult<BlogsDto> GetPaged(int page, int pageSize);
        BlogsDto Create(BlogsDto newBlog);
        void Delete(long id);
        BlogsDto Update(BlogsDto updateBlog);
        BlogsDto Get(long id);
        VoteDto AddVote(VoteDto voteDto);

    }
}