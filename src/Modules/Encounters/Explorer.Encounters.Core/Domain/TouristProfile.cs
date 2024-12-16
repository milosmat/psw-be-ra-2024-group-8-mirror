using Explorer.Stakeholders.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Core.Domain
{
    public class TouristProfile : User
    {
        public TouristProfile(
            string username,
            string password,
            UserRole role,
            bool isActive
        ) : base(username, password, role, isActive)
        {
            XP = 0;
            CompletedEncountersIds = new List<long>();
        }

        public int XP { get; private set; }
        public int Level => XP / 10; // Primer: Svakih 100 XP prelazak na novi nivo
        public List<long> CompletedEncountersIds { get; private set; }

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
    }
}
