using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/comment")]
    public class CommentController : BaseApiController
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public ActionResult<PagedResult<CommentDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _commentService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpPost]
        public ActionResult<CommentDto> Create([FromBody] CommentDto comment)
        {
            var result = _commentService.Create(comment);
            return CreateResponse(result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<CommentDto> Update([FromBody] CommentDto comment)
        {
            var result = _commentService.Update(comment);
            return CreateResponse(result);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var result = _commentService.Delete(id);
            return CreateResponse(result);
        }
    }
}
