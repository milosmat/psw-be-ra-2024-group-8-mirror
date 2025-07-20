using AutoMapper;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.Clubs;
using Explorer.Stakeholders.Core.Domain.TourProblems;

namespace Explorer.Stakeholders.Core.Mappers;

public class StakeholderProfile : Profile
{
    public StakeholderProfile()
    {

        CreateMap<ClubDto, Club>().ReverseMap();
        CreateMap<MembershipRequestDto,MembershipRequest>().ReverseMap();

        CreateMap<MembershipRequestDto, MembershipRequest>().ReverseMap();
           

        CreateMap<AccountInformationDto, User>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse<UserRole>(src.Role)));

        CreateMap<User, AccountInformationDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

        CreateMap<AppRatingDto, AppRating>().ReverseMap();

        CreateMap<ProblemDto, Problem>().ReverseMap();


        CreateMap<TourProblemDto, TourProblem>()
                    .ForMember(dest => dest.ProblemComments, opt => opt.MapFrom(src => src.ProblemComments))
                    .ReverseMap();
        CreateMap<TourProblemDto.ProblemCommentDto, ProblemComment>()
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.TourProblemId, opt => opt.MapFrom(src => src.TourProblemId))
            .ForMember(dest => dest.CommentedAt, opt => opt.MapFrom(src => src.CommentedAt))
            .ReverseMap();

        CreateMap<User, UserDto>();
            //.ReverseMap();


        CreateMap<FollowersDto, Followers>().ReverseMap();

        CreateMap<MessageDto, Message>().ReverseMap();

        CreateMap<NotificationDto, Notification>()
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message)) 
            .ReverseMap();

    }
}