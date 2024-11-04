using AutoMapper;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.Core.Domain;
using Explorer.Blog.Core.Domain.Blogs;

namespace Explorer.Blog.Core.Mappers;

public class BlogProfile : Profile
{
    public BlogProfile()
    {
        CreateMap<BlogsDto, Blogg>().ReverseMap();
        CreateMap<VoteDto,Vote>().ReverseMap();
        CreateMap<CommentDto, Comment>().ReverseMap();
    }
}