using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Tours.API.Dtos;
using FluentResults;
using static Explorer.Tours.API.Dtos.ShoppingCartDTO;

namespace Explorer.Tours.API.Public.Tourist
{
    public interface IShoppingCartService
    {
        void AddTourToCart(long touristId, ShoppingCartItemDTO shoppingCartItemDto);
        void RemoveTourFromCart(long touristId, long tourId);
        ShoppingCartDTO GetShoppingCart(long touristId);
        Result Checkout(long touristId);

    }
}
