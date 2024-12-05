using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public.Tourist;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Explorer.Payments.API.Dtos.ShoppingCartDTO;

namespace Explorer.Payments.Core.UseCases.Tourist
{
    public class CouponService : BaseService<CouponDTO, Coupon>, ICouponService
    {
        public ICouponRepository _couponRepository { get; set; }
        public IMapper _mapper { get; set; }

        public CouponService(ICouponRepository couponRepository, IMapper mapper) : base(mapper)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
        }

        public PagedResult<CouponDTO> GetPaged(int page, int pageSize)
        {
            PagedResult<Coupon> coupons = _couponRepository.GetPaged(page, pageSize);

            var couponDtos = _mapper.Map<List<CouponDTO>>(coupons.Results);
            return new PagedResult<CouponDTO>(couponDtos, coupons.TotalCount);
        }


        public CouponDTO Create(CouponDTO newCoupon)
        {
            return _mapper.Map<CouponDTO>(_couponRepository.Create(_mapper.Map<Coupon>(newCoupon)));
        }


        public void Delete(int id)
        {
            _couponRepository.Delete(id);
        }

        public CouponDTO Update(CouponDTO updateCoupon)
        {
            return _mapper.Map<CouponDTO>(_couponRepository.Update(_mapper.Map<Coupon>(updateCoupon)));
        }

        public CouponDTO Get(int id)
        {
            return _mapper.Map<CouponDTO>(_couponRepository.Get(id));
        }

        public List<ShoppingCartItemDTO> ApplyCouponOnCartItems(string code, List<ShoppingCartItemDTO> cartItems)
        {
            //pronadjemo kupon na osnovu koda
            var coupons = _mapper.Map<List<CouponDTO>>(_couponRepository.GetCouponsByCode(code));
            if (coupons == null || !coupons.Any())
            {
                throw new ArgumentException("Invalid coupon code.");
            }

            if (coupons.Count == 1 && !coupons.First().TourId.HasValue)
            {
                // Prvo nalazimo najskuplju stavku u korpi
                var mostExpensiveItem = cartItems.OrderByDescending(item => item.TourPrice).FirstOrDefault();
                if (mostExpensiveItem == null)
                {
                    throw new ArgumentException("There are not most expensive tour to load.");
                }
                ApplyDiscount(mostExpensiveItem, coupons.First().DiscountPercentage);

            }
            else
            {
                foreach (var cartItem in cartItems)
                {
                    if (coupons.Any(c => c.TourId == cartItem.TourId))
                    {
                        var applicableCoupon = coupons.First(c => c.TourId == cartItem.TourId);
                        ApplyDiscount(cartItem, applicableCoupon.DiscountPercentage);
                    }
                }
            }


            return cartItems;
        }

        public void ApplyDiscount(ShoppingCartItemDTO cartItem, int discountPercentage)
        {

            if (discountPercentage < 0 || discountPercentage > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(discountPercentage), "Discount percentage must be between 0 and 100.");
            }

            
            decimal discountAmount = cartItem.TourPrice * discountPercentage / 100;
            cartItem.TourPrice -= discountAmount;
        }

    }
}
