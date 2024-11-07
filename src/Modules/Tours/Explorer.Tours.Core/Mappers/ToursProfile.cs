using AutoMapper;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.Entities;
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
        CreateMap<TouristEquipmentDTO, TouristEquipment>().ReverseMap();
        CreateMap<VisitedCheckpointDTO, VisitedCheckpoint>().ReverseMap();
        CreateMap<TourPurchaseTokenDTO, TourPurchaseToken>().ReverseMap();



        CreateMap<ObjectDTO, Object>().ReverseMap();
        CreateMap<TourReviewDto, TourReview>().ReverseMap();

        CreateMap<TouristPositionDto, TouristPosition>()
            .ForMember(dest => dest.CurrentLocation, opt => opt.MapFrom(src => new MapLocation(src.CurrentLocation.Latitude, src.CurrentLocation.Longitude)))
            .ReverseMap();


        CreateMap<ShoppingCartDTO, ShoppingCart>()
                 .ForMember(dest => dest.ShopingItems, opt => opt.MapFrom(src => src.ShopingItems))
                 .ReverseMap();

        CreateMap<ShoppingCartDTO.ShoppingCartItemDTO, ShoppingCartItem>()
            .ForMember(dest => dest.TourId, opt => opt.MapFrom(src => src.TourId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TourName))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.TourPrice))
            .ReverseMap();
    }
}