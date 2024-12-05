using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Payments.Core.Domain
{
    public class PaymentRecord : Entity
    {
        public long TouristId { get; private set; }
        public long BundleId { get; private set; }
        public decimal Price { get; private set; }
        public DateTime TransactionTime { get; private set; }

        public PaymentRecord(long touristId, long bundleId, decimal price)
        {
            TouristId = touristId;
            BundleId = bundleId;
            Price = price;
            TransactionTime = DateTime.UtcNow;
        }
    }
}
