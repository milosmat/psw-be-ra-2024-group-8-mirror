using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/tourPreferences")]
    public class TourPreferencesController : BaseApiController
    {
        private readonly ITourPreferenceService _tourPreferenceService;
        
        public TourPreferencesController(ITourPreferenceService tourPreferenceService)
        {
            _tourPreferenceService = tourPreferenceService;
        }

        [HttpPost]
        public ActionResult<TourPreferencesDto> Create([FromBody] TourPreferencesDto tourPreferences)
        {
            var result = _tourPreferenceService.Create(tourPreferences);
            return CreateResponse(result);
        }

        [HttpGet]
        public ActionResult<PagedResult<TourPreferencesDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourPreferenceService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<TourPreferencesDto> Update([FromBody] TourPreferencesDto tourPreferences)
        {
            var result = _tourPreferenceService.Update(tourPreferences);
            return CreateResponse(result);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var result = _tourPreferenceService.Delete(id);
            return CreateResponse(result);
        }
    }
}
