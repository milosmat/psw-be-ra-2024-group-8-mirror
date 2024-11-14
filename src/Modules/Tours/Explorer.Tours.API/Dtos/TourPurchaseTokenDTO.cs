using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class TourPurchaseTokenDTO
    {
        public long TokenId { get; set; }
        public long TouristId { get; set; }
        public long TourId { get; set; }
        public TokenStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public String jwtToken { get; set; }


        public enum TokenStatus
        {
            Active,   // Token je aktivan i može se koristiti
            Used,     // Token je iskorišćen i više nije važeći
            Expired   // Token je istekao i više ne može biti iskorišćen
        }

    }
}
