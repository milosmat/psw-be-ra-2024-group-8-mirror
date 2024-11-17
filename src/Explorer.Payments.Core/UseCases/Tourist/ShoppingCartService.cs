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

namespace Explorer.Payments.Core.UseCases.Tourist
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ICardRepository _cardRepository;
        private readonly ITourPurchaseTokenService _tokenService;

        public ShoppingCartService(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public ShoppingCartService(ICardRepository cardRepository, ITourPurchaseTokenService tokenService)
        {
            _cardRepository = cardRepository;
            _tokenService = tokenService;
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
                TotalPrice = shoppingCart.ShopingItems.Sum(item => item.Price)  // Ukupna cena
            };

            return shoppingCartDto;
        }



        public Result Checkout(long touristId)
        {
            //var shoppingCart = _cardRepository.Get(touristId);
            var shoppingCart = _cardRepository.GetByTouristId(touristId);
            if (shoppingCart == null || !shoppingCart.ShopingItems.Any())
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

            shoppingCart.ShopingItems.Clear();
            //_cardRepository.Update(shoppingCart);
            _cardRepository.Delete(shoppingCart.Id);

            return Result.Ok();
        }

    }
}
