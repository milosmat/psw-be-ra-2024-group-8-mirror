using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.BuildingBlocks.Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    //[Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/blogs")]
    public class BlogsController : BaseApiController
    {
        private readonly IBlogsService _blogsService;

        public BlogsController(IBlogsService blogsService)
        {
            _blogsService = blogsService;
        }



        [HttpGet]
        public ActionResult<PagedResult<BlogsDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            try
            {
                var result = _blogsService.GetPaged(page, pageSize);

                if (result == null || result.Results.Count == 0)
                {
                    return NotFound("No blogs found for the specified page.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching data: {ex.Message}");
            }
        }


        [HttpPost]
        public ActionResult<BlogsDto> Create([FromBody] BlogsDto blog)
        {
            try
            {
                BlogsDto result = _blogsService.Create(blog);

                if (result != null)
                {
                    return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
                }
                else
                {
                    return BadRequest("The blog could not be created due to invalid data.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPut("{id:int}")]
        public ActionResult<BlogsDto> Update([FromBody] BlogsDto blog)
        {
            try
            {
                BlogsDto result = _blogsService.Update(blog);

                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest("The blog could not be created due to invalid data.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var blog = _blogsService.Get(id);

            if (blog == null)
            {
                return NotFound($"Blog with ID {id} not found.");
            }

            _blogsService.Delete(id);

            return Ok($"Blog with ID {id} has been successfully deleted.");
        }

    }
}
