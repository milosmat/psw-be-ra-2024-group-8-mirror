using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.Rating
{
    
    [Route("api/ratings/appRating")]
    public class AppRatingController : BaseApiController
    {
        private readonly IAppRatingService _appRatingService;

        public AppRatingController(IAppRatingService appRatingService)
        {
            _appRatingService = appRatingService;
        }
        [Authorize(Policy = "administratorPolicy")]
        [HttpGet]
        public ActionResult<PagedResult<AppRatingDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize) {
            var result = _appRatingService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }
        [Authorize(Policy = "touristPolicy")]
        [HttpPost]
        public ActionResult<AppRatingDto> Create([FromBody] AppRatingDto appRating)
        {
            var result = _appRatingService.Create(appRating);
            return CreateResponse(result);
        }
        [Authorize(Policy = "administratorPolicy")]
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var result = _appRatingService.Delete(id);
            return CreateResponse(result);
        }
    }
}
