using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.UseCases.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/touristEquipment")]
    public class TouristEquipmentController : BaseApiController
    {
        private readonly ITouristEquipmentService _touristEquipmentService;

        public TouristEquipmentController(ITouristEquipmentService touristEquipmentService)
        {
            _touristEquipmentService = touristEquipmentService;
        }

        [HttpPost]
        public ActionResult<TouristEquipmentDTO> Create([FromBody] TouristEquipmentDTO touristEquipment)
        {
            var result = _touristEquipmentService.Create(touristEquipment);
            return CreateResponse(result);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var result = _touristEquipmentService.Delete(id);
            return CreateResponse(result);
        }

        [HttpGet("{id:int}")]
        public ActionResult<TouristEquipmentDTO> GetById(int id)
        {
            var result = _touristEquipmentService.Get(id);
            return CreateResponse(result);
        }

        [HttpGet("tourist/{touristId:long}/equipment/{equipmentId:long}")]
        public ActionResult<TouristEquipmentDTO> GetByTouristAndEquipment(long touristId, long equipmentId)
        {
            var result = _touristEquipmentService.FindByTouristAndEquipment(touristId, equipmentId);
            return CreateResponse(result);
        }

    }
}
