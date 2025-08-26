using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Payments.API.Dtos;
using Explorer.Tours.API.Dtos;
using FluentResults;
using static Explorer.Payments.API.Dtos.ShoppingCartDTO;

namespace Explorer.Payments.API.Public.Tourist
{
    public interface IShoppingCartService
    {
        List<ShoppingCartItemDto> AddTourToCart(long touristId, ShoppingCartItemDto shoppingCartItemDto);
        bool RemoveTourFromCart(long touristId, long tourId);
        ShoppingCartDTO GetShoppingCart(long touristId);
        Result Checkout(long touristId);
        void AddBoundleToCart(long touristId, ShoppingBundleDto shoppingBoundleDto);
        void RemoveBundleFromCart(long touristId, long bundleId);
        Result<List<BuildingBlocks.Core.Dtos.BundleDTO>> GetBundlesForTourist(long touristId);

        ShoppingCartDTO Update(ShoppingCartDTO updatedShoppingCart);

        Result<ShoppingCartDTO> ApplyCouponToCart(long touristId, string couponCode);
        Result<ShoppingCartDTO> CancelUsedCoupon(long touristId, string couponCode);

    }
}
