using AutoMapper;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Core.Domain;
using Explorer.Blog.Core.Domain.Blogs;
using Explorer.Blog.Core.Domain.RepositoryInterfaces;
using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.Core.UseCases
{
    public class BlogsService : BaseService<BlogsDto, Blogg>, IBlogsService
    {
        private readonly IBlogRepository _blogRepository;
        public BlogsService(IBlogRepository blogRepository, IMapper mapper) : base(mapper)
        {
            _blogRepository = blogRepository;
        }
    }
}