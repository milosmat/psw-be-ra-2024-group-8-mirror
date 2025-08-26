using Explorer.Encounters.API.Dtos;
using FluentResults;
using System.Collections.Generic;

namespace Explorer.Encounters.API.Public.Tourist
{
    public interface ITouristProfileService
    {
        // Dohvata turistu prema ID-u
        Result<TouristProfileDTO> GetTouristById(long id);

        // Dodaje XP turistu
        Result AddXPToTourist(long touristId, int xp);

        // Završava izazov za turistu
        Result CompleteEncounter(long touristId, long encounterId);

        // Dohvata turiste prema nivou
        Result<IEnumerable<TouristProfileDTO>> GetTouristsByLevel(int level);

        // Dohvata listu završenih izazova za turistu
        Result<IEnumerable<EncounterDTO>> GetCompletedEncounters(long touristId);
        Result<TouristProfileDTO> GetTouristByUsername(string username);

        Result<IEnumerable<TouristProfileDTO>> GetAll();

        Result SyncCompletedEncounters(string username);
        // Dodaje ID kupona turističkom profilu
        Result AddCouponToTourist(long touristId, long couponId);

        // Briše ID kupona iz turističkog profila
        Result RemoveCouponFromTourist(long touristId, long couponId);
    }
}
