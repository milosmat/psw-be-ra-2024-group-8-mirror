using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{

    [Route("api/tourist/problemreplies")]
    public class ProblemReplyController : BaseApiController
    {
        private readonly IProblemReplyService _problemReplyService;

        public ProblemReplyController(IProblemReplyService problemReplyService)
        {
            _problemReplyService = problemReplyService;
        }

        [HttpGet]
        public ActionResult<PagedResult<ProblemReplyDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _problemReplyService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpGet("by-problem/{problemId:int}")]
        public ActionResult<PagedResult<ProblemReplyDto>> GetByProblem(int problemId, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _problemReplyService.GetByProblemId(problemId, page, pageSize);
            return CreateResponse(result);
        }

        [HttpPost]
        public ActionResult<ProblemReplyDto> Create([FromBody] ProblemReplyDto reply)
        {
            var result = _problemReplyService.Create(reply);
            return CreateResponse(result);
        }
    }
}
