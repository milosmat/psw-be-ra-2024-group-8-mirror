using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Author;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/pubishledtourss")]
    public class PublishedTours : BaseApiController
    {
        private readonly ITourService _tourService;

        public PublishedTours(ITourService tourService)
        {
            _tourService = tourService;
        }


        [HttpGet]
        public ActionResult<PagedResult<TourDTO>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }
    }
}
