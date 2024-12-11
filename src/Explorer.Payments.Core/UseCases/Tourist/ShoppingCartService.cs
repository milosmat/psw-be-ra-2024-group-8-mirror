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
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.API.Dtos;

namespace Explorer.Payments.Core.UseCases.Tourist
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ICardRepository _cardRepository;
        private readonly ITourPurchaseTokenService _tokenService;
        private readonly IPaymentRecordService _paymentRecordService;
        private readonly IPaymentRecordRepository _paymentRepository;
        private readonly IBundleRepository _bundleRepository;
        private readonly IBundleService _bundleService;
        public ShoppingCartService(
               ICardRepository cardRepository,
               ITourPurchaseTokenService tokenService,
               IPaymentRecordService paymentRecordService,
               IPaymentRecordRepository paymentRepository,
               IBundleRepository bundleRepository,
               IBundleService bundleService
            )
        {
            _cardRepository = cardRepository ?? throw new ArgumentNullException(nameof(cardRepository));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _paymentRecordService = paymentRecordService ?? throw new ArgumentNullException(nameof(paymentRecordService));
            _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
            _bundleRepository = bundleRepository;
            _bundleService = bundleService;
        }


        public void AddTourToCart(long touristId, ShoppingCartItemDTO shoppingCartItemDto)
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



        // Uklanjanje ture iz korpe
        public void RemoveTourFromCart(long touristId, long tourId)
        {
            var shoppingCart = _cardRepository.GetByTouristId(touristId);
            if (shoppingCart != null)
            {
                var itemToRemove = shoppingCart.ShopingItems.FirstOrDefault(item => item.TourId == tourId);
                if (itemToRemove != null)
                {
                    shoppingCart.RemoveItem(itemToRemove);

                    // Spremanje ažurirane korpe u bazu
                    _cardRepository.Update(shoppingCart);
                }
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

            var shoppingCartDto = new ShoppingCartDTO
            {
                TouristId = shoppingCart.TouristId,
                ShopingItems = shoppingCart.ShopingItems.Select(item => new ShoppingCartDTO.ShoppingCartItemDTO
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

            };

            return shoppingCartDto;
        }



        public Result Checkout(long touristId)
        {
            //var shoppingCart = _cardRepository.Get(touristId);
            var shoppingCart = _cardRepository.GetByTouristId(touristId);
            if (shoppingCart == null || (!shoppingCart.ShopingItems.Any() && !shoppingCart.ShopingBundles.Any()))
            {
                return Result.Fail("Cart is empty or does not exist.");
            }

            // Kreiraj instancu TokenService
            var tokenService = new TokenService();

            foreach (var item in shoppingCart.ShopingItems)
            {
                // Kreiraj TourPurchaseTokenDTO
                var tokenDto = new TourPurchaseTokenDTO
                {
                    TouristId = touristId,
                    TourId = item.TourId,
                    Status = TourPurchaseTokenDTO.TokenStatus.Active,
                    CreatedDate = DateTime.UtcNow,
                    ExpiredDate = DateTime.UtcNow.AddYears(1)
                };

                // Generiši JWT token
                var jwtToken = tokenService.GenerateTourPurchaseToken(touristId, item.TourId);  // Poziv za generisanje JWT tokena

                // Dodaj JWT token u DTO
                tokenDto.jwtToken = jwtToken;

                // Kreiraj token u sistemu (spremi ga u bazu ili u neki repository)
                _tokenService.Create(tokenDto);
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

        public Result<List<BundleDTO>> GetBundlesForTourist(long touristId)
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
            var bundles = new List<BundleDTO>();
            foreach (var bundleId in bundleIds)
            {
                // Pozivamo GetBuyBundles metodu koja treba da vrati kupljeni paket
                var bundleResult = _bundleService.Get((int)bundleId);
                if (bundleResult.IsSuccess && bundleResult.Value != null)
                {
                    var bundle = bundleResult.Value;

                    // Dodajemo sve ture za paket
                    var bundleToursResult = _bundleService.GetAllTours(bundle.Id); // Pozivamo metodu koja vraća sve ture za paket
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
            // Fetch the existing shopping cart
            var shoppingCart = _cardRepository.GetByTouristId(updatedShoppingCart.TouristId);
            if (shoppingCart == null)
            {
                throw new InvalidOperationException("Shopping cart not found.");
            }

            // Update the shopping cart with values from the updated DTO
            shoppingCart.ShopingItems = updatedShoppingCart.ShopingItems.Select(itemDto => new ShoppingCartItem(itemDto.TourId, itemDto.TourName, itemDto.TourPrice)).ToList();

            // Save the updated shopping cart back to the repository
            _cardRepository.Update(shoppingCart);

            // Return the updated DTO (map back if needed)
            return new ShoppingCartDTO
            {
                TouristId = shoppingCart.TouristId,
                ShopingItems = shoppingCart.ShopingItems.Select(item => new ShoppingCartDTO.ShoppingCartItemDTO
                {
                    TourId = item.TourId,
                    TourName = item.Name,
                    TourPrice = item.Price
                }).ToList(),
                TotalPrice = shoppingCart.ShopingItems.Sum(item => item.Price)
            };
        }



    }
}
