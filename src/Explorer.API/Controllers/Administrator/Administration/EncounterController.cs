using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public.Administrator;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Explorer.Encounters.API.Controllers
{
    [ApiController]
    [Route("api/administrator/encounters")]
    public class EncounterController : ControllerBase
    {
        private readonly IEncounterService _encounterService;

        public EncounterController(IEncounterService encounterService)
        {
            _encounterService = encounterService;
        }

        // GET: api/administrator/encounters
        [HttpGet]
        public ActionResult<PagedResult<EncounterDTO>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _encounterService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        // GET: api/administrator/encounters/{id}
        [HttpGet("{id:int}")]
        public ActionResult<EncounterDTO> GetById(int id)
        {
            var result = _encounterService.GetById(id);
            return CreateResponse(result);
        }

        // POST: api/administrator/encounters
        [HttpPost]
        public ActionResult<EncounterDTO> Create([FromBody] EncounterDTO encounterDto)
        {
            var result = _encounterService.Create(encounterDto);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
            }
            return BadRequest(result.Errors);
        }

        // PUT: api/administrator/encounters/{id}
        [HttpPut("{id:int}")]
        public ActionResult<EncounterDTO> Update(int id, [FromBody] EncounterDTO encounterDto)
        {
            if (id != encounterDto.Id)
                return BadRequest("ID in URL does not match ID in body.");

            var result = _encounterService.Update(encounterDto);
            return CreateResponse(result);
        }

        // DELETE: api/administrator/encounters/{id}
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var result = _encounterService.Delete(id);
            return CreateResponse(result);
        }

        // POST: api/administrator/encounters/{id}/archive
        [HttpPost("{id:int}/archive")]
        public ActionResult ArchiveEncounter(int id)
        {
            var result = _encounterService.ArchiveEncounter(id);
            return CreateResponse(result);
        }

        // POST: api/administrator/encounters/{id}/publish
        [HttpPost("{id:int}/publish")]
        public ActionResult PublishEncounter(int id)
        {
            var result = _encounterService.PublishEncounter(id);
            return CreateResponse(result);
        }

        private ActionResult CreateResponse(Result result)
        {
            if (result.IsFailed)
            {
                if (result.Errors[0].Message.Contains("not found"))
                    return NotFound(result.Errors);

                return BadRequest(result.Errors);
            }

            return Ok(result);
        }

        private ActionResult<T> CreateResponse<T>(Result<T> result)
        {
            if (result.IsFailed)
            {
                if (result.Errors[0].Message.Contains("not found"))
                    return NotFound(result.Errors);

                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }
    }
}
