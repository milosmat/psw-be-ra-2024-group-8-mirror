using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public.Tourist;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/profiles")]
    public class TouristController : ControllerBase
    {
        private readonly ITouristProfileService _touristProfileService;

        public TouristController(ITouristProfileService touristProfileService)
        {
            _touristProfileService = touristProfileService;
        }

        [HttpGet("{id:long}")]
        public ActionResult<TouristProfileDTO> GetTouristById(long id)
        {
            try
            {
                var result = _touristProfileService.GetTouristById(id);
                return Ok(result);
            }
            catch(KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id:long}/add-xp")]
        public ActionResult AddXPToTourist(long id, [FromBody] int xp)
        {
            if (xp <= 0)
            {
                return BadRequest("XP must be a positive number.");
            }

            var result = _touristProfileService.AddXPToTourist(id, xp);
            return CreateResponse(result);
        }

        [HttpPost("{id:long}/complete-encounter/{encounterId:long}")]
        public ActionResult CompleteEncounter(long id, long encounterId)
        {
            var result = _touristProfileService.CompleteEncounter(id, encounterId);
            return CreateResponse(result);
        }

        [HttpPost("{username}/sync-completed-encounters")]
        public ActionResult SyncCompletedEncounters(string username)
        {
            var result = _touristProfileService.SyncCompletedEncounters(username);
            return CreateResponse(result);
        }


        [HttpGet("level/{level:int}")]
        public ActionResult<IEnumerable<TouristProfileDTO>> GetTouristsByLevel(int level)
        {
            var result = _touristProfileService.GetTouristsByLevel(level);
            return CreateResponse(result);
        }

        [HttpGet("{id:long}/completed-encounters")]
        public ActionResult<IEnumerable<EncounterDTO>> GetCompletedEncounters(long id)
        {
            var result = _touristProfileService.GetCompletedEncounters(id);
            return CreateResponse(result);
        }

        [HttpGet("{username}")]
        public ActionResult<TouristProfileDTO> GetTouristByUsername(string username)
        {
            var result = _touristProfileService.GetTouristByUsername(username);

            if (result.IsFailed)
            {
                return NotFound($"Tourist with username '{username}' not found.");
            }

            return Ok(result.Value);
        }

        // Helper methods for creating ActionResults based on FluentResults
        private ActionResult<T> CreateResponse<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Errors.Select(e => e.Message).ToArray());
        }

        private ActionResult CreateResponse(Result result)
        {
            if (result.IsSuccess)
            {
                return Ok();
            }
            return BadRequest(result.Errors.Select(e => e.Message).ToArray());
        }
    }
}
