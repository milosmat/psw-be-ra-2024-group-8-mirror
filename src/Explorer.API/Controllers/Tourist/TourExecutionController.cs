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

        // Route to start a tour execution
        [HttpPost("start")]
        public ActionResult<TourExecutionDto> StartTourExecution(int tourId, int userId)//, [FromBody] LocationDto startLocationDto)
        {
            var result = _tourExecutionService.StartTourExecution(tourId, userId);//, startLocationDto);
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

        [HttpPost("{executionId:int}/check-visited-checkpoint")]
        public ActionResult CheckVisitedCheckpoint(int executionId, [FromBody] Tours.Core.Domain.MapLocation locationDto)
        {
            var result = _tourExecutionService.CheckForVisitedCheckpoints(executionId, locationDto.Latitude, locationDto.Longitude);
            return CreateResponse(result);
        }

        // Optional route to get a specific tour execution by ID
        /*[HttpGet("{executionId:int}")]
        public ActionResult<TourExecutionDto> GetTourExecution(int executionId)
        {
            var result = _tourExecutionService.GetExecution(executionId);
            return CreateResponse(result);
        }

        // Optional route to get all tour executions (paged)
        [HttpGet]
        public ActionResult<PagedResult<TourExecutionDto>> GetPagedTourExecutions([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourExecutionService.GetPagedExecutions(page, pageSize);
            return CreateResponse(result);
        }

        // Optional route to record an activity (location update)
        [HttpPost("{executionId:int}/record-activity")]
        public ActionResult RecordActivity(int executionId, [FromBody] LocationDto locationDto)
        {
            var result = _tourExecutionService.RecordActivity(executionId, locationDto);
            return CreateResponse(result);
        }*/
    }
}
