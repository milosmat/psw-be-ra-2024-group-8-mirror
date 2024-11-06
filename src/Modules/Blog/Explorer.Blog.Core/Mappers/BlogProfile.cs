using AutoMapper;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.Core.Domain;
using Explorer.Blog.Core.Domain.Blogs;
using System.Linq;

namespace Explorer.Blog.Core.Mappers;

public class BlogProfile : Profile
{
    public BlogProfile()
    {
        CreateMap<BlogsDto, Blogg>().ReverseMap();
        CreateMap<VoteDto,Vote>().ReverseMap();
        //CreateMap<BlogsDto, Blogg>().IncludeAllDerived()
          //  .ForMember(dest => dest.Votes, opt => opt.MapFrom(src =>
            //     src.Votes.Select((h, i) => new Vote(h, Domain.Blogs.Markdown.Upvote))));

        CreateMap<CommentDto, Comment>().ReverseMap();
    }
}