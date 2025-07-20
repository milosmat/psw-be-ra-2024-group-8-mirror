using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public.Tourist;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Payments.Core.UseCases.Tourist
{
    public class CouponService : BaseService<CouponDTO, Coupon>, ICouponService, ICouponSenderService
    {
        public readonly ICouponRepository _couponRepository;
        public readonly ITouristCouponRepository _touristCouponRepository;
        public IMapper _mapper { get; set; }

        public CouponService(ICouponRepository couponRepository,
                             ITouristCouponRepository touristCouponRepository,
                             IMapper mapper) : base(mapper)
        {
            _couponRepository = couponRepository;
            _touristCouponRepository = touristCouponRepository;
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

        public bool ValidateCouponCodeUsage(long touristId, string code)
        {
            return _touristCouponRepository.IsCouponAlreadyUsed(touristId, code);
        }

        private Result<bool> ValidateCoupon(List<Coupon> coupons)
        {

            if (coupons == null || !coupons.Any())
            {
                return Result.Fail(FailureCode.CouponNotFound);
            }
            if (coupons.Any(c => c.ExpiryDate.HasValue && c.ExpiryDate < DateTime.UtcNow))
            {
                Result.Fail(FailureCode.CouponExpired);
            }
            return Result.Ok(true);
        }

        public Result<bool> ApplyCouponOnCartItems(long touristId, string code, List<ShoppingCartItemDto> cartItems)
        {
            var coupons = _couponRepository.GetCouponsByCode(code);
            var availableCoupons = coupons.Where(c => c.IsPublic).ToList();
            var validationResult = ValidateCoupon(availableCoupons);
            if (validationResult.IsFailed)
            {
                return Result.Fail(validationResult.Errors); 
            }

            bool isApplied = false;

            if (availableCoupons.Count == 1 && !availableCoupons.First().TourId.HasValue)
            {
                var coupon = availableCoupons.First();

                bool isWelcomeGift = !coupon.AuthorId.HasValue && coupon.RecipientId.HasValue;

                bool isRecipientValid = (isWelcomeGift && coupon.RecipientId == touristId) /*|| coupon.AuthorId.HasValue*/;

                if (isRecipientValid) //primjenjuje se na ukupnu cijenu 
                {
                    foreach (var item in cartItems)
                    {
                        ApplyDiscount(item, coupon.DiscountPercentage);
                        item.UsedCouponCode = coupon.Code;
                    }
                    isApplied = true;
                }

                else if(coupon.AuthorId.HasValue)
                {
                    var itemSortedByPrice = cartItems.OrderByDescending(item => item.TourPrice).ToList();

                    var mostExpensiveItem = itemSortedByPrice.FirstOrDefault();
                    if (mostExpensiveItem == null)
                    {
                        return Result.Fail(FailureCode.CouponNotApplicable);
                    }

                    var mostExpensiveItems = itemSortedByPrice
                                               .Where(item => item.TourPrice == mostExpensiveItem.TourPrice)
                                               .ToList();


                    foreach (var item in mostExpensiveItems)
                    {
                        ApplyDiscount(item, coupon.DiscountPercentage);
                        item.UsedCouponCode = code;
                    }
                    isApplied = true;
                }

            }
            else /* if(!coupons.Any(c => c.RecipientId.HasValue)) */
            {
                var cartItemsWithCoupons = cartItems.Where
                                           (cartItem => availableCoupons.Any(c => c.TourId == cartItem.TourId)).ToList();

                if (cartItemsWithCoupons.Any())
                {
                    foreach (var cartItem in cartItemsWithCoupons)
                    {
                        var applicableCoupon = availableCoupons.First(c => c.TourId == cartItem.TourId);
                        if(applicableCoupon != null)
                        {
                            ApplyDiscount(cartItem, applicableCoupon.DiscountPercentage);
                            cartItem.UsedCouponCode = applicableCoupon.Code;
                            isApplied = true;
                        }
                    }
                }

            }

            return isApplied ? Result.Ok(true) : Result.Fail(FailureCode.CouponNotApplicable);
        }

        private void ApplyDiscount(ShoppingCartItemDto cartItem, int discountPercentage)
        {
            decimal discountAmount = cartItem.TourPrice * discountPercentage / 100;
            cartItem.TourPriceWithDiscount = cartItem.TourPrice - discountAmount;
        }

        public List<CouponDTO> GetCouponsByIds(List<long> couponIds)
        {
            if (couponIds == null || !couponIds.Any())
            {
                return new List<CouponDTO>();
            }

            var coupons = _couponRepository
                .GetAll()
                .Where(c => couponIds.Contains(c.Id))
                .ToList();

            return _mapper.Map<List<CouponDTO>>(coupons);
        }

        public Result<string> GenerateCouponToNewUser(long userId)
        {
            try
            {
                var couponCode = GenerateRandomCouponCode();
                CouponDTO newCoupon = new CouponDTO
                {
                    Code = couponCode,
                    DiscountPercentage = 5,
                    ExpiryDate = DateTime.UtcNow.AddDays(30),
                    RecipientId = userId
                };
                var createdCoupon = Create(newCoupon);
                if (createdCoupon == null)
                {
                    return Result.Fail("Fail to create coupon.");
                }
                return Result.Ok(createdCoupon.Code);
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred, {ex.Message}");
            }

        }
        private string GenerateRandomCouponCode()
        {
            string couponCode;
            do
            {
                couponCode = Guid.NewGuid().ToString("N").Substring(0, 8);
            }
            while (!_couponRepository.IsCouponCodeUnique(couponCode));

            return couponCode;

        }

        public Result<CouponDTO> MakeCouponPublic(long id)
        {
            try
            {
                var result = _couponRepository.UpdateCouponPublished(id, true);
                return Result.Ok(_mapper.Map<CouponDTO>(result));
            }
            catch(Exception ex) 
            {
                return Result.Fail($"Error: {ex.Message}");
            }
        }

    }
}
