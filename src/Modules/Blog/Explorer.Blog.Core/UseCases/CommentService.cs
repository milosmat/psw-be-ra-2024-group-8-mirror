using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Core.Domain.Blogs;
using Explorer.Blog.Core.Domain.RepositoryInterfaces;
using Explorer.BuildingBlocks.Core.UseCases;
using FluentResults;

namespace Explorer.Blog.Core.UseCases
{
    public class CommentService : ICommentService
    {
        private readonly IBlogRepository _blogRepository;
        public IMapper _mapper;
        
        public CommentService(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public CommentDto Create(long blogId, CommentDto newComment)
        {
            var blog = _blogRepository.GetBlogWithComments(blogId);
            if (blog == null)
            {
                throw new KeyNotFoundException($"Blog with ID {blogId} not found.");
            }
            blog.AddComment(_mapper.Map<Comment>(newComment));
            _blogRepository.Update(blog);
            var lastAddedComment = blog.Comments.Last();
            newComment.Id = lastAddedComment.Id;
            return newComment;

        }

        public void Delete(long blogId, long commentId)
        {
            var blog = _blogRepository.GetBlogWithComments(blogId);
            if (blog == null)
            {
                throw new KeyNotFoundException("Blog not found.");
            }

            blog.DeleteComment(commentId);
            _blogRepository.Update(blog);
        }

        public CommentDto Update(long blogId, CommentDto updatedComment)
        {
            var blog = _blogRepository.GetBlogWithComments(blogId);
            if (blog == null)
            {
                throw new KeyNotFoundException("Blog not found.");
            }

            blog.UpdateComment(_mapper.Map<Comment>(updatedComment));

            _blogRepository.Update(blog);

            return updatedComment;
        }

        public PagedResult<CommentDto> GetPagedByBlog(long blogId, int page, int pageSize)
        {
            var blog = _blogRepository.GetBlogWithComments(blogId);
            if (blog == null)
            {
                throw new KeyNotFoundException("Blog not found.");
            }

            var allComments = blog.Comments?.ToList();

            int totalCount = allComments.Count;

            if (page != 0 || pageSize != 0)
            {
                allComments
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            }


            var commentDtos = _mapper.Map<List<CommentDto>>(allComments);

            return new PagedResult<CommentDto>(commentDtos, totalCount);
        }

        public CommentDto GetById(long commentId, long blogId)
        {
            var blog = _blogRepository.GetBlogWithComments(blogId);
            if (blog == null)
            {
                throw new KeyNotFoundException($"Blog with ID {blogId} not found");
            }

            var comment = blog.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
            {
                throw new KeyNotFoundException($"Comment with ID {commentId} not found");
            }
            return _mapper.Map<CommentDto>(comment);
        }
    }
}  
