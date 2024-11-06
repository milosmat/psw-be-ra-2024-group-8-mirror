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
            PagedResult<Blogg> blogs = _blogRepository.GetPaged(page, pageSize);

            var blogDtos = _mapper.Map<List<BlogsDto>>(blogs.Results);
            return new PagedResult<BlogsDto>(blogDtos, blogs.TotalCount);
        }


        public BlogsDto Create(BlogsDto newBlog)
        {
            return _mapper.Map<BlogsDto>(_blogRepository.Create(_mapper.Map<Blogg>(newBlog)));
        }

        public VoteDto AddVote(VoteDto voteDto)
        {
            Blogg blog = _blogRepository.Get(voteDto.BlogId);
            blog.AddVote(_mapper.Map<Vote>(voteDto));
            _blogRepository.Update(blog);
            return voteDto;
        }

        public void Delete(int id)
        {
            _blogRepository.Delete(id);
        }

        public BlogsDto Update(BlogsDto updateBlog)
        {
            return _mapper.Map<BlogsDto>(_blogRepository.Update(_mapper.Map<Blogg>(updateBlog)));
        }

        public BlogsDto Get(int id)
        {
            return _mapper.Map<BlogsDto>(_blogRepository.Get(id));
        }

        public CommentDto AddComment(long blogId, CommentDto newComment)
        {
            var blog = _blogRepository.Get(blogId);
            if (blog == null)
            {
                throw new Exception("Blog not found");
            }

            var comment = _mapper.Map<Comment>(newComment);
            comment.InitializeComment(); 

            blog.AddComment(comment);
            _blogRepository.Update(blog);

            return _mapper.Map<CommentDto>(comment);
        }

    }
}