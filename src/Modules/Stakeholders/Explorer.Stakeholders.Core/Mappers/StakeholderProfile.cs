using AutoMapper;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;

namespace Explorer.Stakeholders.Core.Mappers;

public class StakeholderProfile : Profile
{
    public StakeholderProfile()
    {
        CreateMap<User, AccountInformationDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => (int)src.Role));
      
        CreateMap<Person, AccountInformationDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));


        CreateMap<AccountInformationDto, User>()
            .ForMember(dest => dest.Role, opt => opt.Ignore()); 
      
        CreateMap<AccountInformationDto, Person>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore()) 
            .ForMember(dest => dest.Name, opt => opt.Ignore()) 
            .ForMember(dest => dest.Surname, opt => opt.Ignore());
    }
}