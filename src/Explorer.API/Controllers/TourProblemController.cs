using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Author;
using Microsoft.AspNetCore.Mvc;
using static Explorer.Stakeholders.API.Dtos.TourProblemDto;

namespace Explorer.API.Controllers
{
    [Route("api/tourProblem")]
    public class TourProblemController : BaseApiController
    {
        private readonly ITourProblemService _tourProblemService;
        private readonly ITourService _tourService;

        public TourProblemController(ITourProblemService tourProblemService, ITourService tourService)
        {
            _tourProblemService = tourProblemService;
            _tourService = tourService;
        }

        [HttpGet("author-stats")]
        public ActionResult<List<AuthorStatsDto>> GetAuthorStatistics()
        {
            var result = _tourProblemService.GetAuthorStatistics();
            return CreateResponse(result);
        }

        [HttpGet("tourForTourProblem/{id:int}")]
        public ActionResult<TourDTO> GetById(int id)
        {
            var result = _tourService.Get(id);
            return CreateResponse(result);
        }

        [HttpGet("findUser/{id:int}")]
        public ActionResult<UserDto> GetUser(int id)
        {
            var result = _tourProblemService.GetUser(id);
            return CreateResponse(result);
        }

        [HttpPost("{problemId:int}/comments")]
        public ActionResult AddProblemComment(int problemId, [FromBody] ProblemCommentDto problemCommentDto)
        {
            var result = _tourProblemService.AddProblemComment(problemId, problemCommentDto);
            return CreateResponse(result);
        }

        [HttpGet("forUser/{userId:long}")]
        public ActionResult<List<TourProblemDto>> GetAllForUser(long userId)
        {
            var result = _tourProblemService.GetAllForUser(userId);
            return CreateResponse(result);
        }

        [HttpPost("update")]
        public ActionResult<TourProblemDto> Update([FromBody] TourProblemDto tourProblemDto)
        {
            var result = _tourProblemService.Update(tourProblemDto);
            return CreateResponse(result);
        }

        [HttpPost]
        public ActionResult<TourProblemDto> Create([FromBody] TourProblemDto tourProblemDto)
        {
            var result = _tourProblemService.Create(tourProblemDto);
            return CreateResponse(result);
        }
    }
}
