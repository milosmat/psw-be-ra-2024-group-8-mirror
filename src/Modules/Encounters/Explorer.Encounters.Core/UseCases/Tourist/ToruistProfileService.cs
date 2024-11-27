using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using Explorer.Encounters.API.Public.Tourist;
using FluentResults;
using System.Collections.Generic;
using System.Linq;

namespace Explorer.Encounters.Core.UseCases.Tourist
{
    public class TouristProfileService : CrudService<TouristProfileDTO, TouristProfile>, ITouristProfileService
    {
        private readonly ITouristProfileRepository _touristRepository;
        private readonly IMapper _mapper;

        public TouristProfileService(ITouristProfileRepository touristRepository, IMapper mapper)
            : base(touristRepository, mapper)
        {
            _touristRepository = touristRepository;
            _mapper = mapper;
        }

        public Result<TouristProfileDTO> GetTouristById(long id)
        {
            var tourist = _touristRepository.Get(id);
            if (tourist == null)
            {
                return Result.Fail("Tourist not found.");
            }

            var touristDto = _mapper.Map<TouristProfileDTO>(tourist);
            return Result.Ok(touristDto);
        }

        public Result AddXPToTourist(long touristId, int xp)
        {
            var tourist = _touristRepository.Get(touristId);
            if (tourist == null)
            {
                return Result.Fail("Tourist not found.");
            }

            tourist.AddXP(xp);
            _touristRepository.Update(tourist);

            return Result.Ok();
        }

        public Result CompleteEncounter(long touristId, long encounterId)
        {
            var tourist = _touristRepository.Get(touristId);
            if (tourist == null)
            {
                return Result.Fail("Tourist not found.");
            }

            if (tourist.IsEncounterCompleted(encounterId))
            {
                return Result.Fail("Encounter already completed.");
            }

            tourist.CompleteEncounter(encounterId);
            _touristRepository.Update(tourist);

            return Result.Ok();
        }

        public Result<IEnumerable<TouristProfileDTO>> GetTouristsByLevel(int level)
        {
            var tourists = _touristRepository.GetAll()
                .Where(t => t.Level == level);

            if (!tourists.Any())
            {
                return Result.Fail("No tourists found at the specified level.");
            }

            var touristDtos = _mapper.Map<IEnumerable<TouristProfileDTO>>(tourists);
            return Result.Ok(touristDtos);
        }

        public Result<IEnumerable<EncounterDTO>> GetCompletedEncounters(long touristId)
        {
            var tourist = _touristRepository.Get(touristId);
            if (tourist == null)
            {
                return Result.Fail("Tourist not found.");
            }

            // Mapiranje završenih izazova na DTO
            var completedEncounters = tourist.CompletedEncountersIds
                .Select(id => new EncounterDTO { Id = id });

            return Result.Ok(completedEncounters);
        }

        public Result<TouristProfileDTO> GetTouristByUsername(string username)
        {
            // Dohvatanje turističkog profila prema korisničkom imenu
            var tourist = _touristRepository.GetByUsername(username);
            if (tourist == null)
            {
                return Result.Fail($"Tourist with username '{username}' not found.");
            }

            // Mapiranje turističkog profila na DTO
            var touristDto = _mapper.Map<TouristProfileDTO>(tourist);
            return Result.Ok(touristDto);
        }
    }
}
