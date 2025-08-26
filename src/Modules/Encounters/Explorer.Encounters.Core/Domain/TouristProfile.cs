using Explorer.Stakeholders.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace Explorer.Encounters.Core.Domain
{
    public class TouristProfile : User
    {
        public TouristProfile(
            string username,
            string passwordHash,
            UserRole role,
            bool isActive
        ) : base(username, passwordHash, role, isActive)
        {
            XP = 0;
            CompletedEncountersIds = new List<long>();
            CouponIds = new List<long>(); // Initialize the coupon ID list
        }

        public int XP { get; private set; }
        public int Level => XP / 10; // Primer: Svakih 100 XP prelazak na novi nivo
        public List<long> CompletedEncountersIds { get; private set; }
        public List<long> CouponIds { get; private set; }
        public void AddXP(int amount)
        {
            if (amount <= 0) throw new ArgumentException("XP mora biti pozitivan broj.");
            XP += amount;
        }

        public void CompleteEncounter(long encounterId)
        {
            if (!CompletedEncountersIds.Contains(encounterId))
                CompletedEncountersIds.Add(encounterId);
        }
        public bool IsEncounterCompleted(long encounterId)
        {
            return CompletedEncountersIds.Contains(encounterId);
        }
        // Method to add a coupon ID
        public void AddCouponId(long couponId)
        {
            if (couponId <= 0) throw new ArgumentException("Coupon ID must be positive.");
            if (!CouponIds.Contains(couponId))
            {
                CouponIds.Add(couponId);
            }
        }
        // Method to remove a coupon ID
        public bool RemoveCouponId(long couponId)
        {
            return CouponIds.Remove(couponId);
        }
    }
}
