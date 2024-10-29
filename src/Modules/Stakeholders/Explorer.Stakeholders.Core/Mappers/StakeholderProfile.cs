using AutoMapper;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;

namespace Explorer.Stakeholders.Core.Mappers;

public class StakeholderProfile : Profile
{
    public StakeholderProfile()
    {
    CreateMap<ClubDto, Club>().ReverseMap();

    CreateMap<AccountInformationDto, User>()
        .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse<UserRole>(src.Role)));

    CreateMap<User, AccountInformationDto>()
        .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

    CreateMap<AppRatingDto, AppRating>().ReverseMap();

    CreateMap<ProblemDto, Problem>().ReverseMap();

    CreateMap<ProblemReplyDto, ProblemReply>().ReverseMap();
    }
}