using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentResults;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/tour-executions")]
    public class TourExecutionController : BaseApiController
    {
        private readonly ITourExecutionService _tourExecutionService;

        public TourExecutionController(ITourExecutionService tourExecutionService)
        {
            _tourExecutionService = tourExecutionService;
        }

        [HttpPost("start")]
        public ActionResult<TourExecutionDto> StartTourExecution([FromQuery] int tourId, [FromQuery] int userId)
        {
            // Proveravamo da li su `tourId` i `userId` validni
            if (tourId <= 0 || userId <= 0)
            {
                return BadRequest("Invalid tour or user ID.");
            }

            var result = _tourExecutionService.StartTourExecution(tourId, userId);
            return CreateResponse(result);
        }

        // Route to complete a tour execution
        [HttpPost("{executionId:int}/complete")]
        public ActionResult CompleteTourExecution(int executionId)
        {
            var result = _tourExecutionService.CompleteTourExecution(executionId);
            return CreateResponse(result);
        }

        // Route to abandon a tour execution
        [HttpPost("{executionId:int}/abandon")]
        public ActionResult AbandonTourExecution(int executionId)
        {
            var result = _tourExecutionService.AbandonTourExecution(executionId);
            return CreateResponse(result);
        }


        [HttpGet("all-tours")]
        public ActionResult<TourDTO> GetAllTours()
        {
            var result = _tourExecutionService.GetAllTours();
            return CreateResponse(result);
        }

        [HttpPost("{executionId:int}/check-visited-checkpoint")]
        public ActionResult CheckVisitedCheckpoint(int executionId, [FromBody] Tours.Core.Domain.MapLocation locationDto)
        {
            var result = _tourExecutionService.CheckForVisitedCheckpoints(executionId, locationDto.Latitude, locationDto.Longitude);
            return CreateResponse(result);
        }

        [HttpGet("status")]
        public ActionResult<TourExecutionDto> GetTourExecutionStatus([FromQuery] int tourId, [FromQuery] int userId)
        {
            if (tourId <= 0 || userId <= 0)
            {
                return BadRequest("Invalid tour or user ID.");
            }

            var result = _tourExecutionService.GetTourExecutionStatus(tourId, userId);
            return CreateResponse(result);
        }
    }
}
