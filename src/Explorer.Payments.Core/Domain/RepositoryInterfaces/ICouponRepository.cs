using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain.RepositoryInterfaces
{
    public interface ICouponRepository : ICrudRepository<Coupon>
    {
        PagedResult<Coupon> GetPaged(int page, int pageSize);
        Coupon Create(Coupon newCoupon);
        void Delete(long id);
        Coupon Update(Coupon updateCoupon);
        Coupon Get(int id);
    }
}
