using AutoMapper;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Core.Domain;
using Explorer.Blog.Core.Domain.Blogs;
using Explorer.Blog.Core.Domain.RepositoryInterfaces;
using Explorer.BuildingBlocks.Core.UseCases;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.Core.UseCases
{
    public class BlogsService : BaseService<BlogsDto, Blogg>, IBlogsService
    {
        public IBlogRepository _blogRepository {  get; set; }
        public IMapper _mapper { get; set; }

        public BlogsService(IBlogRepository blogRepository, IMapper mapper) : base(mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }
        public Result Vote(int id, Vote voteDto)
        {
            throw new NotImplementedException();
        }

        public PagedResult<BlogsDto> GetPaged(int page, int pageSize)
        {
            return null;
        }

        public BlogsDto Create(BlogsDto newBlog)
        {
            return _mapper.Map<BlogsDto>(_blogRepository.Create(_mapper.Map<Blogg>(newBlog)));
        }

        public void Delete(long id)
        {
            _blogRepository.Delete(id);
        }

        public BlogsDto Update(BlogsDto updateBlog)
        {
            return _mapper.Map<BlogsDto>(_blogRepository.Update(_mapper.Map<Blogg>(updateBlog)));
        }

        public BlogsDto Get(long id)
        {
            return _mapper.Map<BlogsDto>(_blogRepository.Get(id));
        }
    }
}