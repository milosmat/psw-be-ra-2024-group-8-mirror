using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator.Administration
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/author/tour-checkpoints")]
    public class TourCheckpointController : BaseApiController
    {
        private readonly ITourCheckpointService _tourCheckpointService;

        public TourCheckpointController(ITourCheckpointService tourCheckpointService)
        {
            _tourCheckpointService = tourCheckpointService;
        }

        [HttpGet]
        public ActionResult<PagedResult<TourCheckpointDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourCheckpointService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpPost]
        public ActionResult<TourCheckpointDto> Create([FromBody] TourCheckpointDto tourCheckpoint)
        {
            var result = _tourCheckpointService.Create(tourCheckpoint);
            return CreateResponse(result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<TourCheckpointDto> Update([FromBody] TourCheckpointDto tourCheckpoint)
        {
            var result = _tourCheckpointService.Update(tourCheckpoint);
            return CreateResponse(result);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var result = _tourCheckpointService.Delete(id);
            return CreateResponse(result);
        }
    }
}
