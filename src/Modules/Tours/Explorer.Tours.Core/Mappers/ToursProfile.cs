using AutoMapper;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;
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

        CreateMap<TouristEquipmentDTO, TouristEquipment>().ReverseMap();

        CreateMap<ObjectDTO, Object>().ReverseMap();
        CreateMap<TourReviewDto, TourReview>().ReverseMap();

    }
}