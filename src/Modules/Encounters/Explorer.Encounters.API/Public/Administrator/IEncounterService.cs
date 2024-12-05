using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Public.Administrator
{
    public interface IEncounterService
    {
        // CRUD operacije za Encounter
        Result<PagedResult<EncounterDTO>> GetPaged(int page, int pageSize);
        Result<EncounterDTO> GetById(int id);
        Result<EncounterDTO> Create(EncounterDTO encounterDto);
        Result<EncounterDTO> Update(EncounterDTO encounterDto);
        Result Delete(int id);
        Result ArchiveEncounter(int id);
        Result PublishEncounter(int id);
        Result MarkEncounterAsReviewed(int encounterId);
    }
}
