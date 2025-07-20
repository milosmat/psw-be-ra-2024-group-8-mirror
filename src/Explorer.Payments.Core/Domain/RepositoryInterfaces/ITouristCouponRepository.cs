using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain.RepositoryInterfaces
{
    public interface ITouristCouponRepository: ICrudRepository<TouristCoupon>
    {
        public TouristCoupon Create(TouristCoupon newEntity);
        public bool IsCouponAlreadyUsed(long touristId, string couponCode);
    }
}
