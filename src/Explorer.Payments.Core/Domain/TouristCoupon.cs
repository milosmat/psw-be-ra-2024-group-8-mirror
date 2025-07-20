using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain
{
    public class TouristCoupon: Entity
    {
        public long TouristId { get; private set; }
        public string CouponCode { get;  private set; }

        public TouristCoupon() { }

        public TouristCoupon(long touristId, string couponCode)
        {
            TouristId = touristId;
            CouponCode = couponCode;
        }
    }
}
