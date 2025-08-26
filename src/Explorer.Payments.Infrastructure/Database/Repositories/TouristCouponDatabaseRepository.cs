using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Infrastructure.Database.Repositories
{
    public class TouristCouponDatabaseRepository:CrudDatabaseRepository<TouristCoupon, PaymentsContext>, ITouristCouponRepository
    {
        public TouristCouponDatabaseRepository(PaymentsContext dbContext): base(dbContext) { }

        public bool IsCouponAlreadyUsed(long touristId, string couponCode)
        {
            return DbContext.TouristsCoupons.Any(tc => tc.TouristId == touristId && tc.CouponCode.Equals(couponCode));
        }
    }
}
