using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator.Administration
{
    [Authorize(Policy = "administratorPolicy")]
    [Route("api/administration/problems")]
    public class AllProblemsController : BaseApiController
    {
        private readonly IProblemService _equipmentService;

        public AllProblemsController(IProblemService equipmentService)
        {
            _equipmentService = equipmentService;
        }

        [HttpGet]
        public ActionResult<PagedResult<ProblemDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _equipmentService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }


    }
}
