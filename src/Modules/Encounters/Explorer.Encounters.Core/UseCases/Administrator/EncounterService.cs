using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public.Administrator;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
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
        private readonly IMapper _mapper;

        public EncounterService(ICrudRepository<Encounter> repository, IMapper mapper, IEncounterRepository encounterRepository)
            : base(repository, mapper)
        {
            _mapper = mapper;
            _encounterRepository = encounterRepository;
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

        public new Result<EncounterDTO> Create(EncounterDTO encounterDto)
        {
            var encounter = _mapper.Map<Encounter>(encounterDto);
            var createdEncounter = _encounterRepository.Create(encounter);

            if (createdEncounter == null)
            {
                return Result.Fail("Failed to create Encounter.");
            }

            return Result.Ok(_mapper.Map<EncounterDTO>(createdEncounter));
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

        public new Result Delete(int id)
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
        public Result ArchiveEncounter(int encounterId)
        {
            var encounter = _encounterRepository.Get(encounterId);
            if (encounter == null)
                return Result.Fail("Encounter not found.");

            encounter.Archive(); // Koristi metod iz klase Encounter
            _encounterRepository.Update(encounter); // Ažuriraj u bazi

            return Result.Ok();
        }

        // Publish Encounter
        public Result PublishEncounter(int encounterId)
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
        public Result<EncounterDTO> GetById(int id)
        {
            var encounter = _encounterRepository.Get(id);
            if (encounter == null)
            {
                return Result.Fail("Encounter not found.");
            }

            return Result.Ok(_mapper.Map<EncounterDTO>(encounter));
        }
    }
}
