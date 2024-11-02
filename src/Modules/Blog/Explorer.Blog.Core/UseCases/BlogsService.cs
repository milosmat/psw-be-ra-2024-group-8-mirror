using AutoMapper;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Core.Domain;
using Explorer.Blog.Core.Domain.Blogs;
using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.Core.UseCases
{
    public class BlogsService : CrudService<BlogsDto,Blogg>, IBlogsService
    {
        public BlogsService(ICrudRepository<Blogg> repository, IMapper mapper) : base(repository, mapper) { }
    }
}