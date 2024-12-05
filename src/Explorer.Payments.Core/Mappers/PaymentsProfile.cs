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
        CreateMap<CouponDTO,Coupon>().ReverseMap();

        CreateMap<PaymentRecordDTO, PaymentRecord>().ReverseMap();


        CreateMap<ShoppingCartDTO, ShoppingCart>()
      .ForMember(dest => dest.ShopingItems, opt => opt.MapFrom(src => src.ShopingItems))
      .ForMember(dest => dest.ShopingBundles, opt => opt.MapFrom(src => src.ShopingBundles))
      .ReverseMap();

        CreateMap<ShoppingCartDTO.ShoppingCartItemDTO, ShoppingCartItem>()
            .ForMember(dest => dest.TourId, opt => opt.MapFrom(src => src.TourId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TourName))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.TourPrice))
            .ReverseMap();

        CreateMap<ShoppingCartDTO.ShoppingBundleDto, ShoppingCartBundle>()
           .ForMember(dest => dest.BundleId, opt => opt.MapFrom(src => src.BundleId))
           .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
           .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
           .ReverseMap();

        CreateMap<WalletDTO.TransactionItemsDTO, Transaction>()
            .ForMember(dest=> dest.AdministratorId, otp => otp.MapFrom(src=> src.AdministratorId))
            .ForMember(dest=> dest.TransactionTime, otp => otp.MapFrom(src=> src.TransactionTime))
            .ForMember(dest=> dest.Amount, otp=> otp.MapFrom(src=> src.Amount))
            .ForMember(dest=> dest.Description, otp=>otp.MapFrom(src=> src.Description))
            .ReverseMap();


    }
}
