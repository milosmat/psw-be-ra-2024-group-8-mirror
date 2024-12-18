using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Tours.API.Dtos;
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
        Result<EncounterDTO> GetById(long id);
        Result<EncounterDTO> Create(EncounterDTO encounterDto);
        Result<EncounterDTO> Update(EncounterDTO encounterDto);
        Result Delete(long id);
        Result ArchiveEncounter(long id);
        Result PublishEncounter(long id);
        Result CheckTouristsInEncounters();
        Result MarkEncounterAsReviewed(long encounterId);

    }
}
