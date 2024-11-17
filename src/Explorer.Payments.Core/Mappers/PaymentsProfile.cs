using AutoMapper;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Mappers;

public class PaymentsProfile : Profile
{
    public PaymentsProfile() {

        CreateMap<TourPurchaseTokenDTO, TourPurchaseToken>().ReverseMap();

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
