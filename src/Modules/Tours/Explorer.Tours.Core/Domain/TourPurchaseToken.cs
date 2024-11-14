using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain
{
    public class TourPurchaseToken : Entity
    {
        public long TouristId { get; private set; }
        public long TourId { get; private set; }
        public TokenStatus Status { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime ExpiredDate { get; private set; }
        public String jwtToken { get; private set; }

        public TourPurchaseToken(long touristId, long tourId, String jwtToken)
        {
            TouristId = touristId;
            TourId = tourId;
            Status = TokenStatus.Active;
            CreatedDate = DateTime.UtcNow;
            ExpiredDate = CreatedDate.AddYears(1);  // Token važi godinu dana od datuma kreiranja
            this.jwtToken = jwtToken;

            Validate();
        }

        public void UseToken()
        {
            if (Status != TokenStatus.Active)
                throw new InvalidOperationException("Token se ne može koristiti jer nije aktivan.");

            if (DateTime.UtcNow > ExpiredDate)
            {
                Status = TokenStatus.Expired;
                throw new InvalidOperationException("Token je istekao i ne može se koristiti.");
            }

            Status = TokenStatus.Used;
        }

        private void Validate()
        {
            if (TouristId <= 0)
                throw new ArgumentException("ID turiste mora biti validan.");

            if (TourId <= 0)
                throw new ArgumentException("ID ture mora biti validan.");
        }

        public enum TokenStatus
        {
            Active,   // Token je aktivan i može se koristiti
            Used,     // Token je iskorišćen i više nije važeći
            Expired   // Token je istekao i više ne može biti iskorišćen
        }

    }
}
