using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/position")]
    public class TouristPositionController : ControllerBase
    {
        private readonly ITouristPositionService _touristPositionService;

        public TouristPositionController(ITouristPositionService touristPositionService)
        {
            _touristPositionService = touristPositionService;
        }

        [HttpGet("{touristId:int}")]
        public ActionResult<TouristPositionDto> GetPosition(int touristId)
        {
            var result = _touristPositionService.GetPosition(touristId);
            return CreateResponse(result);
        }
        [HttpPost("{touristId:int}")]
        public ActionResult<TouristPositionDto> SetPosition(int touristId, [FromBody] TouristPositionDto positionDto)
        {
            if (positionDto.CurrentLocation == null)
            {
                return BadRequest("Location data is missing.");
            }

            var result = _touristPositionService.SetPosition(
                touristId,
                positionDto.CurrentLocation.Latitude,
                positionDto.CurrentLocation.Longitude
            );

            return CreateResponse(result);
        }

        // Helper method to create ActionResult based on FluentResults
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
