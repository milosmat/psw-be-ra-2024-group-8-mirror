using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Explorer.Payments.API.Dtos.ShoppingCartDTO;

namespace Explorer.Payments.API.Public.Tourist
{
    public interface ICouponService
    {
        PagedResult<CouponDTO> GetPaged(int page, int pageSize);
        CouponDTO Create(CouponDTO newCoupon);
        void Delete(int id);
        CouponDTO Update(CouponDTO updateCoupon);
        CouponDTO Get(int id);
        List<ShoppingCartItemDto> ApplyCouponOnCartItems(string code, List<ShoppingCartItemDto> cartItems);
        List<CouponDTO> GetCouponsByIds(List<long> couponIds);
    }
}
