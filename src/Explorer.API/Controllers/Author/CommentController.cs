using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author
{
    
    [Route("api/author/blog/{blogId}/comment")]
    public class CommentController : BaseApiController
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public ActionResult<PagedResult<CommentDto>> GetAll([FromRoute] long blogId,[FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _commentService.GetPagedByBlog(blogId, page, pageSize);
            if (result == null || !result.Results.Any())
            {
                return NotFound("No comments found for the specified blog.");
            }

            return Ok(result);
        }

        [Authorize(Policy = "authorPolicy")]
        [HttpPost]
        public ActionResult<CommentDto> Create([FromRoute]long blogId,[FromBody] CommentDto comment)
        {
            try
            {
                var createdComment = _commentService.Create(blogId, comment);
                return CreatedAtAction(nameof(Create), new { blogId, commentId = createdComment.Id }, createdComment);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Policy = "authorPolicy")]
        [HttpPut("{id:int}")]
        public ActionResult<CommentDto> Update([FromRoute]long blogId, long id, [FromBody] CommentDto updatedComment)
        {
            if (id != updatedComment.Id)
            {
                return BadRequest("Comment ID mismatch.");
            }
            try
            {
                var result = _commentService.Update(blogId, updatedComment);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
           
        }

        [Authorize(Policy = "authorPolicy")]
        [HttpDelete("{id:int}")]
        public ActionResult Delete([FromRoute]long blogId,int id)
        {
            try
            {
                _commentService.Delete(blogId, id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }
    }
}
