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
           // var result = _blogsService.GetPaged(page, pageSize);
            return Ok(true);
        }

        [HttpPost]
        public ActionResult<BlogsDto> Create([FromBody] BlogsDto blog)
        {
          //  var result = _blogsService.Create(blog);
            return Ok(true);
        }

        [HttpPut("{id:int}")]
        public ActionResult<BlogsDto> Update([FromBody] BlogsDto blog)
        {
           // var result = _blogsService.Update(blog);
            return Ok(true);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
        //    var result = _blogsService.Delete(id);
            return Ok(true);
        }

    }
}
