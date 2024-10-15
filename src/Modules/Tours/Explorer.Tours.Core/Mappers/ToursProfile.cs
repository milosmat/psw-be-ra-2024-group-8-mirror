using AutoMapper;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;
using System.Text.Json;
using TransportMode = Explorer.Tours.API.Dtos.TransportMode;

namespace Explorer.Tours.Core.Mappers;

public class ToursProfile : Profile
{
    public ToursProfile()
    {
        CreateMap<EquipmentDto, Equipment>().ReverseMap();
        CreateMap<TourPreferencesDto, TourPreferences>().ReverseMap();

        CreateMap<TourPreferencesDto, TourPreferences>()
            .ForMember(dest => dest.TransportPreferences, opt => opt.MapFrom(src => src.TransportPreferences))
            .ForMember(dest => dest.InterestTags, opt => opt.MapFrom(src => src.InterestTags))
            .ReverseMap();
    }
}