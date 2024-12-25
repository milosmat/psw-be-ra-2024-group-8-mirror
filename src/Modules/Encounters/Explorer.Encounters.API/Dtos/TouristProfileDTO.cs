using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Dtos
{
    public class TouristProfileDTO
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public int XP { get; set; }
        public int Level { get; set; }
        public List<long> CompletedEncountersIds { get; set; }
        public List<long> CouponIds { get; set; } // Added to store coupon IDs

        public TouristProfileDTO()
        {
            CompletedEncountersIds = new List<long>();
            CouponIds = new List<long>();
        }
    }
}
