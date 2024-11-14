using AutoMapper;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.Core.Domain.Blogs;
using System.Linq;

namespace Explorer.Blog.Core.Mappers;

public class BlogProfile : Profile
{
    public BlogProfile()
    {
        //  CreateMap<BlogsDto, Blogg>().ReverseMap();
        //   CreateMap<VoteDto,Vote>().ReverseMap();
        CreateMap<BlogsDto, Blogg>().IncludeAllDerived()
            .ForMember(dest => dest.Votes, opt => opt.MapFrom(src => src.Votes.Select(v => new Vote(v.UserId, v.BlogId, (Domain.Blogs.Markdown)v.Mark))));

        CreateMap<Blogg, BlogsDto>()
                .ForMember(dest => dest.Votes, opt => opt.MapFrom(src => src.Votes.Select(v => new VoteDto
                        {
                            UserId = v.UserId,
                            BlogId = v.BlogId,
                            Mark = (API.Dtos.Markdown)v.Mark
                })));

        //CreateMap<VoteDto, Vote>()
        //.ConstructUsing(v => new Vote(v.UserId, v.BlogId, (Domain.Blogs.Markdown)v.Mark));

        CreateMap<VoteDto, Vote>().ReverseMap();
        CreateMap<CommentDto, Comment>().ReverseMap();
    }
}