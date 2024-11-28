using AutoMapper;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.ValueObjects;
using Object = Explorer.Tours.Core.Domain.Object;

namespace Explorer.Tours.Core.Mappers;

public class ToursProfile : Profile
{
    public ToursProfile()
    {
        CreateMap<EquipmentDto, Equipment>().ReverseMap();
        CreateMap<TourPreferencesDto, TourPreferences>().ReverseMap();
        CreateMap<TourCheckpointDto, TourCheckpoint>().ReverseMap();
        CreateMap<TourDTO, Tour>().ReverseMap();
        CreateMap<TourExecutionDto, TourExecution>().ReverseMap();
        CreateMap<TravelTimeDTO, TravelTime>().ReverseMap();
        CreateMap<DailyAgendaDTO, DailyAgenda>().ReverseMap();
        CreateMap<TouristEquipmentDTO, TouristEquipment>().ReverseMap();
        CreateMap<VisitedCheckpointDTO, VisitedCheckpoint>().ReverseMap();
        CreateMap<TourSaleDto, TourSale>().ReverseMap();



        CreateMap<ObjectDTO, Object>().ReverseMap();
        CreateMap<TourReview, TourReviewDto>()
            .ForMember(dest => dest.Personn, opt => opt.MapFrom(src => src.Personn))
            .ReverseMap();
        CreateMap<TouristPositionDto, TouristPosition>()
            .ForMember(dest => dest.CurrentLocation, opt => opt.MapFrom(src => new MapLocation(src.CurrentLocation.Latitude, src.CurrentLocation.Longitude)))
            .ReverseMap();
    }
}