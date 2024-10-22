using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Author;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/author/objects")]
    public class ObjectController : BaseApiController
    {
        private readonly IObjectService _objectService;

        public ObjectController(IObjectService objectService)
        {
            _objectService = objectService;
        }

        [HttpGet]
        public ActionResult<PagedResult<ObjectDTO>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _objectService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ObjectDTO> GetById(int id)
        {
            var result = _objectService.Get(id);
            return CreateResponse(result);
        }

        [HttpPost]
        public ActionResult<ObjectDTO> Create([FromBody] ObjectDTO objectDto)
        {
            var result = _objectService.Create(objectDto);
            return CreateResponse(result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ObjectDTO> Update([FromBody] ObjectDTO objectDto)
        {
            var result = _objectService.Update(objectDto);
            return CreateResponse(result);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var result = _objectService.Delete(id);
            return CreateResponse(result);
        }
    }
}
