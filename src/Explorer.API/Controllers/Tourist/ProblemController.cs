using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{

    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/problems")]
    public class ProblemController : BaseApiController
    {
        private readonly IProblemService _equipmentService;

        public ProblemController(IProblemService equipmentService)
        {
            _equipmentService = equipmentService;
        }


        [HttpGet]
        public ActionResult<PagedResult<ProblemDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _equipmentService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }


        [HttpPost]
        public ActionResult<ProblemDto> Create([FromBody] ProblemDto equipment)
        {
            var result = _equipmentService.Create(equipment);
            return CreateResponse(result);
        }

    }
}
