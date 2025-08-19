using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public.Tourist;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Payments.Core.Domain;
using FluentResults;
using static Explorer.Payments.API.Dtos.ShoppingCartDTO;
using Explorer.Tours.API.Public.Author;
using Explorer.Tours.API.Dtos;
using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Core.Dtos;
using Explorer.Tours.Core.Domain;

namespace Explorer.Payments.Core.UseCases.Tourist
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ICardRepository _cardRepository;
        private readonly ITourPurchaseTokenService _tokenService;
        private readonly IPaymentRecordService _paymentRecordService;
        private readonly IPaymentRecordRepository _paymentRepository;
        // private readonly IBundleService _bundleService;
        private readonly IPaymentBundleService _paymentBundleService;
        private readonly ICouponService _couponService;
        private readonly ITouristCouponRepository _touristCouponRepository;
        public IMapper _mapper { get; set; }
        public ShoppingCartService(
               ICardRepository cardRepository,
               ITourPurchaseTokenService tokenService,
               IPaymentRecordService paymentRecordService,
               IPaymentRecordRepository paymentRepository,
               //IBundleService bundleService,
               IPaymentBundleService paymentBundleService,
               ICouponService couponService,
               ITouristCouponRepository touristCouponRepository,
               IMapper mapper
            )
        {
            _cardRepository = cardRepository ?? throw new ArgumentNullException(nameof(cardRepository));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _paymentRecordService = paymentRecordService ?? throw new ArgumentNullException(nameof(paymentRecordService));
            _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
            //_bundleService = bundleService;
            _paymentBundleService = paymentBundleService;
            _couponService = couponService;
            _touristCouponRepository = touristCouponRepository;
            _mapper = mapper;
        }


        public List<ShoppingCartItemDto> AddTourToCart(long touristId, ShoppingCartItemDto shoppingCartItemDto)
        {
            // Pretraži korpu za turistu
            var shoppingCart = _cardRepository.GetByTouristId(touristId);

            // Ako korpa ne postoji, kreiraj novu
            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart(touristId);
                _cardRepository.Create(shoppingCart);  // Spremi novu korpu u bazu
            }

            // Kreiraj stavku u korpi sa informacijama iz DTO-a
            var shoppingCartItem = new ShoppingCartItem(shoppingCartItemDto.TourId, shoppingCartItemDto.TourName, shoppingCartItemDto.TourPrice);

            // Dodaj stavku u korpu
            shoppingCart.AddItem(shoppingCartItem);

            // Spremi ažuriranu korpu u bazu
            _cardRepository.Update(shoppingCart);

            return _mapper.Map<List<ShoppingCartItemDto>>(shoppingCart.ShopingItems);

        }

        public void AddBoundleToCart(long touristId, ShoppingBundleDto shoppingBoundleDto)
        {
            // Pretraži korpu za turistu
            var shoppingCart = _cardRepository.GetByTouristId(touristId);

            // Ako korpa ne postoji, kreiraj novu
            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart(touristId);
                _cardRepository.Create(shoppingCart);
            }

            // Kreiraj stavku u korpi sa informacijama iz DTO-a
            var shoppingCartBoundle = new ShoppingCartBundle(shoppingBoundleDto.BundleId, shoppingBoundleDto.Name, shoppingBoundleDto.Price);

            // Dodaj stavku u korpu
            shoppingCart.AddBundle(shoppingCartBoundle);

            // Spremi ažuriranu korpu u bazu
            _cardRepository.Update(shoppingCart);
        }

        public bool RemoveTourFromCart(long touristId, long tourId)
        {
            try
            {
                var shoppingCart = _cardRepository.GetByTouristId(touristId);
                if (shoppingCart == null)
                {
                    return false;
                }

                var itemToRemove = shoppingCart.ShopingItems.FirstOrDefault(item => item.TourId == tourId);
                if (itemToRemove == null)
                {
                    return false;
                }
                shoppingCart.RemoveItem(itemToRemove);

                _cardRepository.Update(shoppingCart);
                return true;
            }
            catch (InvalidOperationException ex)
            {
                return false;
            }

        }

        public void RemoveBundleFromCart(long touristId, long bundleId)
        {
            var shoppingCart = _cardRepository.GetByTouristId(touristId);
            if (shoppingCart != null)
            {
                var itemToRemove = shoppingCart.ShopingBundles.FirstOrDefault(item => item.BundleId == bundleId);
                if (itemToRemove != null)
                {
                    shoppingCart.RemoveBundle(itemToRemove);

                    // Spremanje ažurirane korpe u bazu
                    _cardRepository.Update(shoppingCart);
                }
            }
        }

        public ShoppingCartDTO GetShoppingCart(long touristId)
        {
            // Direktno pretraživanje korpe po turistu, umesto paginacije
            var shoppingCart = _cardRepository.GetByTouristId(touristId);

            if (shoppingCart == null)
            {
                return null;
            }

            var shoppingCartDto = _mapper.Map<ShoppingCartDTO>(shoppingCart);
            /*var shoppingCartDto = new ShoppingCartDTO
            {
                TouristId = shoppingCart.TouristId,
                ShopingItems = shoppingCart.ShopingItems.Select(item => new ShoppingCartItemDto
                {
                    TourId = item.TourId,
                    TourName = item.Name,
                    TourPrice = item.Price
                }).ToList(),


                ShopingBundles = shoppingCart.ShopingBundles.Select(bundle => new ShoppingCartDTO.ShoppingBundleDto
                {
                    BundleId = bundle.BundleId,
                    Name = bundle.Name,
                    Price = bundle.Price
                }).ToList(),

                // Ukupna cena (sabira ture i pakete)
                TotalPrice = shoppingCart.ShopingItems.Sum(item => item.Price) + shoppingCart.ShopingBundles.Sum(bundle => bundle.Price)

            };*/

            return shoppingCartDto;
        }



        public Result Checkout(long touristId)
        {
            var shoppingCart = _cardRepository.GetByTouristId(touristId);
            if (shoppingCart == null || (!shoppingCart.ShopingItems.Any() && !shoppingCart.ShopingBundles.Any()))
            {
                return Result.Fail("Cart is empty or does not exist.");
            }

            // Kreiraj instancu TokenService
            var tokenService = new TokenService();

            foreach (var item in shoppingCart.ShopingItems)
            {
                int discountPercentage = 0;
                if (item.Price > 0 && item.TourPriceWithDiscount.HasValue)
                {
                    discountPercentage = (int)(100 * (item.Price - item.TourPriceWithDiscount.Value) / item.Price);
                }

                // Kreiraj TourPurchaseTokenDTO
                var tokenDto = new TourPurchaseTokenDTO
                {
                    TouristId = touristId,
                    TourId = item.TourId,
                    Status = TourPurchaseTokenDTO.TokenStatus.Active,
                    CreatedDate = DateTime.UtcNow,
                    ExpiredDate = DateTime.UtcNow.AddYears(1),
                    TourPrice = item.Price,
                    FinalTourPrice = item.TourPriceWithDiscount ?? item.Price,
                    DiscountPercentage = discountPercentage

                };

                // Generiši JWT token
                var jwtToken = tokenService.GenerateTourPurchaseToken(touristId, item.TourId);  // Poziv za generisanje JWT tokena

                // Dodaj JWT token u DTO
                tokenDto.jwtToken = jwtToken;


                // Kreiraj token u sistemu (spremi ga u bazu ili u neki repository)
                _tokenService.Create(tokenDto);

                CollectUsedCouponsInPurchase(touristId, _mapper.Map<List<ShoppingCartItemDto>>(shoppingCart.ShopingItems));

            }

            foreach (var bundle in shoppingCart.ShopingBundles)
            {
                // Kreiraj PaymentRecord za svaki paket
                var paymentRecord = new PaymentRecord(touristId, bundle.BundleId, bundle.Price);

                // Spremi zapis o plaćanju u bazu/repozitorijum
                _paymentRepository.Create(paymentRecord);
            }

            shoppingCart.ShopingItems.Clear();
            shoppingCart.ShopingBundles.Clear();
            //_cardRepository.Update(shoppingCart);
            _cardRepository.Delete(shoppingCart.Id);



            return Result.Ok();
        }

        public Result<List<BuildingBlocks.Core.Dtos.BundleDTO>> GetBundlesForTourist(long touristId)
        {
            // Dobij sve zapise o plaćanju za datog korisnika
            var paymentRecords = _paymentRepository.GetAllByTouristId(touristId);

            if (paymentRecords == null || !paymentRecords.Any())
            {
                return Result.Fail("No payment records found for the tourist.");
            }

            // Prikupi ID-ove paketa
            var bundleIds = paymentRecords.Select(record => record.BundleId).ToList();

            // Dohvati sve pakete iz servisa
            var bundles = new List<BuildingBlocks.Core.Dtos.BundleDTO>();
            foreach (var bundleId in bundleIds)
            {
                // Pozivamo GetBuyBundles metodu koja treba da vrati kupljeni paket
                var bundleResult = _paymentBundleService.Get((int)bundleId);
                if (bundleResult.IsSuccess && bundleResult.Value != null)
                {
                    var bundle = bundleResult.Value;

                    // Dodajemo sve ture za paket
                    var bundleToursResult = _paymentBundleService.GetAllTours(bundle.Id); // Pozivamo metodu koja vraća sve ture za paket
                    if (bundleToursResult.IsSuccess && bundleToursResult.Value != null)
                    {
                        bundle.Tours = bundleToursResult.Value;
                    }

                    bundles.Add(bundle);
                }
            }

            if (!bundles.Any())
            {
                return Result.Fail("No purchased bundles found for the provided tourist.");
            }

            return Result.Ok(bundles);
        }


        public ShoppingCartDTO Update(ShoppingCartDTO updatedShoppingCart)
        {

            var shoppingCart = _cardRepository.GetByTouristId(updatedShoppingCart.TouristId);
            if (shoppingCart == null)
            {
                throw new InvalidOperationException("Shopping cart not found.");
            }


            shoppingCart.ShopingItems = _mapper.Map<List<ShoppingCartItem>>(updatedShoppingCart.ShopingItems);
            shoppingCart.CalculateTotalPrice();
            _cardRepository.Update(shoppingCart);

            var result = _mapper.Map<ShoppingCartDTO>(shoppingCart);
            //result.TotalPrice = result.ShopingItems.Sum(item => item.TourPriceWithDiscount ?? item.TourPrice);
            return result;
        }


        public void CollectUsedCouponsInPurchase(long touristId, List<ShoppingCartItemDto> purchasedTours)
        {
            // Filtriranje samo onih koje imaju kupon
            var toursWithCoupons = purchasedTours.Where(tour => !string.IsNullOrEmpty(tour.UsedCouponCode))
                                    .DistinctBy(tour => tour.TourId)
                                    .ToList();

            if (toursWithCoupons.Any())
            {

                foreach (var tour in toursWithCoupons)
                {
                    if (!_touristCouponRepository.IsCouponAlreadyUsed(touristId, tour.UsedCouponCode))
                    {
                        _touristCouponRepository.Create(new TouristCoupon(touristId, tour.UsedCouponCode));
                    }

                }

            }
        }

        public Result<ShoppingCartDTO> ApplyCouponToCart(long touristId, string couponCode)
        {
            try
            {
                if (string.IsNullOrEmpty(couponCode))
                {
                    return Result.Fail(FailureCode.InvalidArgument).WithError("Coupon code is required.");
                }

                var shoppingCart = GetShoppingCart(touristId);
                if (shoppingCart == null)
                {
                    return Result.Fail(FailureCode.NotFound).WithError("Shopping cart is not found.");
                }

                if (_couponService.ValidateCouponCodeUsage(touristId, couponCode))
                {
                    return Result.Fail(FailureCode.BadRequest).WithError("You have already used this coupon.");
                }

                var applyingResult = _couponService.ApplyCouponOnCartItems(touristId, couponCode, shoppingCart.ShopingItems);
                if (applyingResult.IsFailed)
                {
                    return Result.Fail(applyingResult.Errors.Last());
                }

                return Result.Ok(Update(shoppingCart));
            }
            catch (Exception e)
            {
                return Result.Fail("An internal error occurred while processing the request.");
            }

        }

        public Result<ShoppingCartDTO> CancelUsedCoupon(long touristId, string couponCode)
        {
            try
            {
                if (string.IsNullOrEmpty(couponCode))
                {
                    return Result.Fail(FailureCode.InvalidArgument).WithError("Coupon code is required.");
                }

                var shoppingCart = GetShoppingCart(touristId);
                if (shoppingCart == null)
                {
                    return Result.Fail(FailureCode.NotFound).WithError("Shopping cart is not found.");
                }

                var shoppingCartItems = shoppingCart.ShopingItems;

                if (shoppingCartItems == null || !shoppingCartItems.Any())
                {
                    return Result.Fail(FailureCode.NotFound).WithError("No shopping items found in the cart.");
                }

                var discountedCartItems = shoppingCartItems.
                                            Where(item => item.UsedCouponCode?.Equals(couponCode) == true)
                                            .ToList();
                if(!discountedCartItems.Any())
                {
                    return Result.Fail(FailureCode.CouponNotFound);
                }

                foreach (var item in discountedCartItems)
                {
                    item.UsedCouponCode = null;
                    item.TourPriceWithDiscount = null;
                }

                return Result.Ok(Update(shoppingCart));
            }
            catch (Exception e)
            {
                return Result.Fail("An internal error occurred while processing the request.");
            }
        }
    }
}
