using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Author;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Explorer.Tours.API.Public.Administration;

namespace Explorer.API.Controllers.Author
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/author/tours")]
    public class TourController : BaseApiController
    {
       private readonly ITourService _tourService;
       private readonly IEquipmentService _equipmentService;

        public TourController(ITourService tourService, IEquipmentService equipmentService)
        {
            _tourService = tourService;
            _equipmentService = equipmentService;
        }

        [HttpGet]
        public ActionResult<PagedResult<TourDTO>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpGet("equipment/{id:int}")]
        public ActionResult<EquipmentDto> GetEquipment(int id)
        {
            var result = _equipmentService.Get(id);
            return CreateResponse(result);
        }

        [HttpGet("{id:int}")]
        public ActionResult<TourDTO> GetById(int id)
        {
            var result = _tourService.Get(id);
            return CreateResponse(result);
        }

        [HttpPost]
        public ActionResult<TourDTO> Create([FromBody] TourDTO tourDto)
        {
            var result = _tourService.Create(tourDto);
            return CreateResponse(result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<TourDTO> Update([FromBody] TourDTO tourDto)
        {
            var result = _tourService.Update(tourDto);
            return CreateResponse(result);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var result = _tourService.Delete(id);
            return CreateResponse(result);
        }

        [HttpGet("{id:int}/equipment-ids")]
        public ActionResult<List<long>> GetEquipmentIds(int id)
        {
            var result = _tourService.GetEquipmentIds(id);
            return CreateResponse(result);
        }

        [HttpPost("{id:int}/equipment-ids/{equipmentId:long}")]
        public ActionResult AddEquipmentId(int id, long equipmentId)
        {
            var result = _tourService.AddEquipmentId(id, equipmentId);
            return CreateResponse(result);
        }

        [HttpDelete("{id:int}/equipment-ids/{equipmentId:long}")]
        public ActionResult RemoveEquipmentId(int id, long equipmentId)
        {
            var result = _tourService.RemoveEquipmentId(id, equipmentId);
            return CreateResponse(result);
        }

        [HttpGet("{id:int}/checkpoint-ids")]
        public ActionResult<List<long>> GetCheckpointIds(int id)
        {
            var result = _tourService.GetCheckpointIds(id);
            return CreateResponse(result);
        }

        [HttpPost("{id:int}/checkpoint-ids/{checkpointId:long}")]
        public ActionResult AddCheckpointId(int id, long checkpointId)
        {
            var result = _tourService.AddCheckpointId(id, checkpointId);
            return CreateResponse(result);
        }

        [HttpDelete("{id:int}/checkpoint-ids/{checkpointId:long}")]
        public ActionResult RemoveCheckpointId(int id, long checkpointId)
        {
            var result = _tourService.RemoveCheckpointId(id, checkpointId);
            return CreateResponse(result);
        }

    }
}
