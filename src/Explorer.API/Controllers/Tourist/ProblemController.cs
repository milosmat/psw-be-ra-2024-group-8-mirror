using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{

    
    [Route("api/tourist/problems")]
    public class ProblemController : BaseApiController
    {
        private readonly IProblemService _problemService;
        private readonly IProblemReplyService _problemReplyService;

        public ProblemController(IProblemService problemService, IProblemReplyService problemReplyService)
        {
            _problemService = problemService;
            _problemReplyService = problemReplyService;
        }

        [HttpGet("by-tourist/{touristId:int}")]
        public ActionResult<PagedResult<ProblemDto>> GetAllByTourist(int touristId, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _problemService.GetByTouristId(touristId, page, pageSize);
            return CreateResponse(result);
        }

        [HttpGet]
        public ActionResult<PagedResult<ProblemDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _problemService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpGet("replies/{problemId:int}")]
        public ActionResult<PagedResult<ProblemDto>> GetAllReplies(int problemId, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _problemReplyService.GetByProblemId(problemId, page, pageSize);
            return CreateResponse(result);
        }

        [Authorize(Policy = "touristPolicy")]
        [HttpPost]
        public ActionResult<ProblemDto> Create([FromBody] ProblemDto problem)
        {
            var result = _problemService.Create(problem);
            return CreateResponse(result);
        }

        [HttpPost("/reply")]
        public ActionResult<ProblemReplyDto> RespondToProblem([FromBody] ProblemReplyDto problemReply)
        {
            var result = _problemReplyService.Create(problemReply);
            return CreateResponse(result);
        }

        [HttpPost("problemSolved")]
        public ActionResult<ProblemDto> ProblemSolved([FromBody] ProblemDto problem)
        {
            var result = _problemService.Update(problem);
            return CreateResponse(result);
        }

        [HttpPost("problemUnsolved")]
        public ActionResult<ProblemDto> ProblemUnsolved([FromBody] ProblemDto problem)
        {
            var result = _problemService.Update(problem);
            return CreateResponse(result);
        }
    }
}
