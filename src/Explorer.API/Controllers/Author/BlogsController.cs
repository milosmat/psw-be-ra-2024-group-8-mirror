using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.BuildingBlocks.Core.UseCases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Explorer.API.Controllers.Author
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/author/blog")]
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
            var result = _blogsService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpPost]
        public ActionResult<BlogsDto> Create([FromBody] BlogsDto blog)
        {
            var result = _blogsService.Create(blog);
            return CreateResponse(result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<BlogsDto> Update([FromBody] BlogsDto blog)
        {
            var result = _blogsService.Update(blog);
            return CreateResponse(result);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var result = _blogsService.Delete(id);
            return CreateResponse(result);
        }

    }
}
