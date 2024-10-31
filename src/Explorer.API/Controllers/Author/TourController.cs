using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Author;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Explorer.Tours.Core.Domain;
using FluentResults;
using Explorer.Tours.API.Public.Administration;

namespace Explorer.API.Controllers.Author
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/author/tours")]
    public class TourController : BaseApiController
    {
        private readonly ITourService _tourService;

        private readonly IEquipmentService _equipmentService;
        private readonly ITourCheckpointService _tourCheckpointService;
        public TourController(ITourService tourService, IEquipmentService equipmentService, ITourCheckpointService checkpointService)
        {
            _tourService = tourService;
            _equipmentService = equipmentService;
            _tourCheckpointService = checkpointService;
        }
        [HttpGet]
        public ActionResult<PagedResult<TourDTO>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourService.GetPaged(page, pageSize);
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

        // Route to add equipment to a tour
        [HttpPost("{id:int}/equipment")]
        public ActionResult AddEquipment(int id, [FromBody] EquipmentDto equipmentDto)
        {
            var result = _tourService.AddEquipment(id, equipmentDto);
            return CreateResponse(result);
        }

        // Route to remove equipment from a tour
        [HttpDelete("{id:int}/equipment")]
        public ActionResult RemoveEquipment(int id, [FromBody] EquipmentDto equipmentDto)
        {
            var result = _tourService.RemoveEquipment(id, equipmentDto);
            return CreateResponse(result);
        }

        // Route to add a checkpoint to a tour
        [HttpPost("{id:int}/checkpoints")]
        public ActionResult AddCheckpoint(int id, [FromBody] TourCheckpointDto checkpointDto)
        {
            var result = _tourService.AddCheckpoint(id, checkpointDto);
            return CreateResponse(result);
        }

        // Route to remove a checkpoint from a tour
        [HttpDelete("{id:int}/checkpoints")]
        public ActionResult RemoveCheckpoint(int id, [FromBody] TourCheckpointDto checkpointDto)
        {
            var result = _tourService.RemoveCheckpoint(id, checkpointDto);
            return CreateResponse(result);
        }
    }
}
