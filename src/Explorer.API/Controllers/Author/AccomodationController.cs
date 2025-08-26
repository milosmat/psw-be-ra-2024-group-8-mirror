using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Author;
using Explorer.Tours.Core.UseCases.Author;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author
{
    //[Authorize(Policy = "adminPolicy")]
    [Route("api/admin/accomodations")]
    public class AccomodationController : BaseApiController
    {
        private readonly IAccomodationService _accomodationService;

        public AccomodationController(IAccomodationService accomodationService)
        {
            _accomodationService = accomodationService;
        }

        [HttpGet]
        public ActionResult<PagedResult<AccomodationDTO>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _accomodationService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ObjectDTO> GetById(int id)
        {
            var result = _accomodationService.Get(id);
            return CreateResponse(result);
        }

        [HttpPost]
        public ActionResult<ObjectDTO> Create([FromBody] AccomodationDTO accomodationDto)
        {
            var result = _accomodationService.Create(accomodationDto);
            return CreateResponse(result);
        }
    }
}
