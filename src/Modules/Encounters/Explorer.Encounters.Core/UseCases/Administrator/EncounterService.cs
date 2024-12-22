using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public.Administrator;
using Explorer.Encounters.API.Public.Tourist;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Core.UseCases.Administrator
{
    public class EncounterService : CrudService<EncounterDTO, Encounter>, IEncounterService
    {
        private readonly IEncounterRepository _encounterRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITouristProfileService _touristProfileService;
        private readonly ITouristPositionService _touristPositionService;
        private readonly IMapper _mapper;

        public EncounterService(ICrudRepository<Encounter> repository, IMapper mapper, IEncounterRepository encounterRepository, ITouristProfileService touristProfileService,
            IUserRepository userRepository, ITouristPositionService touristPositionService)
            : base(repository, mapper)
        {
            _mapper = mapper;
            _encounterRepository = encounterRepository;
            _touristProfileService = touristProfileService;
            _userRepository = userRepository;
            _touristPositionService = touristPositionService; 
        }

        // CRUD Operations
        public new Result<PagedResult<EncounterDTO>> GetPaged(int page, int pageSize)
        {
            var pagedEncounters = _encounterRepository.GetPaged(page, pageSize);
            if (pagedEncounters == null || !pagedEncounters.Results.Any())
            {
                return Result.Fail("No encounters found.");
            }

            var encounterDtos = pagedEncounters.Results.Select(e => _mapper.Map<EncounterDTO>(e)).ToList();
            return Result.Ok(new PagedResult<EncounterDTO>(encounterDtos, pagedEncounters.TotalCount));
        }

        public new Result<EncounterDTO> Update(EncounterDTO encounterDto)
        {
            var encounter = _encounterRepository.Get(encounterDto.Id);
            if (encounter == null)
            {
                return Result.Fail("Encounter not found.");
            }

            _mapper.Map(encounterDto, encounter);
            var updatedEncounter = _encounterRepository.Update(encounter);

            return updatedEncounter != null
                ? Result.Ok(_mapper.Map<EncounterDTO>(updatedEncounter))
                : Result.Fail("Failed to update Encounter.");
        }

        public new Result Delete(long id)
        {
            try
            {
                _encounterRepository.Delete(id);
                return Result.Ok();
            }
            catch (KeyNotFoundException)
            {
                return Result.Fail("Encounter not found.");
            }
        }

        // Archive Encounter
        public Result ArchiveEncounter(long encounterId)
        {
            var encounter = _encounterRepository.Get(encounterId);
            if (encounter == null)
                return Result.Fail("Encounter not found.");

            encounter.Archive(); // Koristi metod iz klase Encounter
            _encounterRepository.Update(encounter); // Ažuriraj u bazi

            return Result.Ok();
        }

        // Publish Encounter
        public Result PublishEncounter(long encounterId)
        {
            var encounter = _encounterRepository.Get(encounterId);
            if (encounter == null)
                return Result.Fail("Encounter not found.");

            var result = encounter.Publish(); // Koristi metod Publish koji vraća Result
            if (result.IsFailed)
                return result;

            _encounterRepository.Update(encounter); // Ažuriraj u bazi

            return Result.Ok();
        }

        // Get Encounter By Id
        public Result<EncounterDTO> GetById(long id)
        {
            var encounter = _encounterRepository.Get(id);
            if (encounter == null)
            {
                return Result.Fail("Encounter not found.");
            }

            return Result.Ok(_mapper.Map<EncounterDTO>(encounter));
        }

        // Override za kreiranje, da postavi default vrednosti za nove parametre
        public new Result<EncounterDTO> Create(EncounterDTO encounterDto)
        {
            var encounter = _mapper.Map<Encounter>(encounterDto);
            if (encounter.Type == EncounterType.SOCIAL)
            {
                encounter.SetSocialEncounterData(encounterDto.RequiredParticipants ?? 0, encounterDto.Radius ?? 0);
            }
            encounter.SetPublishedDateNow();
            var createdEncounter = _encounterRepository.Create(encounter);

            if (createdEncounter == null)
            {
                return Result.Fail("Failed to create Encounter.");
            }

            return Result.Ok(_mapper.Map<EncounterDTO>(createdEncounter));
        }

        private bool IsWithinRadius(MapLocation encounterLocation, MapLocation touristLocation, int radius)
        {
            var distance = CalculateDistance(encounterLocation.Latitude, encounterLocation.Longitude,
                                             touristLocation.Latitude, touristLocation.Longitude);
            return distance <= radius;
        }

        // Haversine formula za računanje udaljenosti između dve GPS lokacije
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double EarthRadiusKm = 6371.0;
            var dLat = DegreesToRadians(lat2 - lat1);
            var dLon = DegreesToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return EarthRadiusKm * c * 1000; // Konvertuj u metre
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        public IEnumerable<Encounter> GetAllActiveByType(EncounterType type)
        {
            int page = 1;
            int pageSize = 50; // Ili neka druga vrednost pogodna za vašu aplikaciju
            var result = new List<Encounter>();

            while (true)
            {
                var pagedResult = _encounterRepository.GetPaged(page, pageSize);

                if (pagedResult == null || !pagedResult.Results.Any())
                    break;

                // Filtriraj samo aktivne izazove traženog tipa
                var filteredResults = pagedResult.Results
                    .Where(e => e.Type == type && e.Status == EncounterStatus.ACTIVE);

                result.AddRange(filteredResults);

                if (pagedResult.Results.Count < pageSize)
                    break; // Ako smo dobili manje rezultata od veličine stranice, nema više stranica

                page++;
            }

            return result;
        }

        public Result CheckTouristsInEncounters()
        {
            // Dohvatanje svih turističkih profila
            var touristsResult = _touristProfileService.GetAll();
            if (touristsResult.IsFailed)
            {
                return Result.Fail("Failed to retrieve tourist profiles.");
            }

            var touristProfiles = touristsResult.Value;

            // Dohvatanje korisnika na osnovu korisničkih imena iz turističkih profila
            var users = new List<User>();
            foreach (var tourist in touristProfiles)
            {
                var user = _userRepository.GetActiveByName(tourist.Username);
                if (user != null)
                {
                    users.Add(user);
                }
            }

            if (!users.Any())
            {
                return Result.Fail("No active users found.");
            }

            // Dohvatanje lokacija svih korisnika
            var touristPositions = new List<TouristPositionDto>();
            foreach (var user in users)
            {
                var positionResult = _touristPositionService.GetPosition((int)user.Id);
                if (positionResult.IsSuccess)
                {
                    touristPositions.Add(positionResult.Value);
                }
            }

            if (!touristPositions.Any())
            {
                return Result.Fail("No tourist positions found.");
            }

            // Dohvatanje svih aktivnih SOCIAL encounter-a
            var activeSocialEncounters = GetAllActiveByType(EncounterType.SOCIAL);

            foreach (var encounter in activeSocialEncounters)
            {
                // Proveravamo turiste koji su u blizini trenutnog encounter-a
                var touristsInRange = touristPositions.Where(tp =>
                    IsWithinRadius(encounter.Location,
                                   new MapLocation
                                   {
                                       Latitude = tp.CurrentLocation.Latitude,
                                       Longitude = tp.CurrentLocation.Longitude
                                   },
                                   encounter.Radius.GetValueOrDefault())).ToList();

                // Ako su uslovi za završetak encounter-a ispunjeni
                if (touristsInRange.Count >= encounter.RequiredParticipants.GetValueOrDefault())
                {
                    foreach (var tourist in touristsInRange)
                    {
                        var user = users.FirstOrDefault(u => u.Id == tourist.TouristId);
                        if (user != null)
                        {
                            var userProfile = _touristProfileService.GetTouristByUsername(_userRepository.GetUser(user.Id).Username);
                            // Provera da li je encounter već završen
                            var completedEncountersResult = _touristProfileService.GetCompletedEncounters(userProfile.Value.Id);
                            if (completedEncountersResult.IsFailed)
                            {
                                Console.WriteLine($"Failed to retrieve completed encounters for user {user.Id}: {completedEncountersResult.Errors.FirstOrDefault()?.Message}");
                                continue; // Preskočimo ovog korisnika ako ne možemo dobiti zavšene encounter-e
                            }

                            var completedEncounters = completedEncountersResult.Value;
                            if (!completedEncounters.Any(e => e.Id == encounter.Id))
                            {
                                // Ako encounter nije završen, obeležavamo ga kao završen
                                encounter.AddUserToCompleted(user.Id);
                                Console.WriteLine($"Users completed: {string.Join(", ", encounter.UsersWhoCompletedId)}");
                                _touristProfileService.CompleteEncounter(userProfile.Value.Id, encounter.Id);
                                _touristProfileService.AddXPToTourist(userProfile.Value.Id, encounter.XP);
                            }
                        }
                    }

                    _encounterRepository.Update(encounter); // Ažuriranje encounter-a
                }
            }

            return Result.Ok();
        }

        public Result MarkEncounterAsReviewed(long encounterId)
        {
            // Dohvatanje Encounter-a iz baze
            var encounter = _encounterRepository.Get(encounterId);
            if (encounter == null)
            {
                return Result.Fail($"Encounter with ID {encounterId} not found.");
            }

            // Pozivanje metode za označavanje kao pregledan
            var result = encounter.MarkAsReviewed();
            if (result.IsFailed)
            {
                return result; // Ako već pregledan, vraća poruku o grešci
            }

            // Ažuriranje u bazi
            _encounterRepository.Update(encounter);
            return Result.Ok();
        }

    }
}
