using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Author;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/author/tours")]
    public class TourController : BaseApiController
    {

       private readonly ITourService _tourService;
       
       private readonly IEquipmentService _equipmentService;
        private readonly ITourCheckpointService _tourCheckpointService;
        


        public TourController(ITourService tourService)
        {
            _tourService = tourService;
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

        [HttpPost("{tourId:int}/checkpoint")]
        public ActionResult<TourCheckpointDto> AddNewCheckpoint([FromBody] TourCheckpointDto checkpoint, long tourId)
        {
            
            var result = _tourService.AddNewCheckpoint(tourId, checkpoint);
            return CreateResponse(result);

        }
        [HttpPost("{tourId:int}/addNewTravelTime")]
        public ActionResult<TravelTimeDTO> AddNewTravelTime1([FromBody] TravelTimeDTO newTravelTime, long tourId)
        {
            var result = _tourService.AddNewTravelTime(tourId, newTravelTime);
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

        // Route to get all equipment
        [HttpGet("equipment")]
        public ActionResult<PagedResult<EquipmentDto>> GetAllEquipment([FromQuery] int page, [FromQuery] int pageSize)
        {

            var result = _tourService.GetPagedEquipment(page, pageSize);
            return CreateResponse(result);

        }

        // Route to create new equipment
        [HttpPost("equipment")]
        public ActionResult<EquipmentDto> CreateEquipment([FromBody] EquipmentDto equipmentDto)
        {
            var result = _tourService.CreateEquipment(equipmentDto);
            return CreateResponse(result);
        }

        // Route to update equipment
        [HttpPut("equipment/{id:int}")]
        public ActionResult<EquipmentDto> UpdateEquipment(int id, [FromBody] EquipmentDto equipmentDto)
        {
            var result = _tourService.UpdateEquipment(id, equipmentDto);
            return CreateResponse(result);
        }

        // Route to delete equipment
        [HttpDelete("equipment/{id:int}")]
        public ActionResult DeleteEquipment(int id)
        {
            var result = _tourService.DeleteEquipment(id);
            return CreateResponse(result);
        }

        // Route to get equipment by ID
        [HttpGet("equipment/{id:int}")]
        public ActionResult<EquipmentDto> GetEquipmentById(int id)
        {
            var result = _tourService.GetEquipment(id);
            return CreateResponse(result);
        }

        // Route to get all checkpoints
        [HttpGet("checkpoints")]
        public ActionResult<PagedResult<TourCheckpointDto>> GetAllCheckpoints([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourService.GetPagedCheckpoint(page, pageSize);
            return CreateResponse(result);
        }

        // Route to create new checkpoint
        [HttpPost("checkpoints")]
        public ActionResult<TourCheckpointDto> CreateCheckpoint([FromBody] TourCheckpointDto tourCheckpointDto)
        {
            var result = _tourService.CreateCheckpoint(tourCheckpointDto);
            return CreateResponse(result);
        }

        // Route to get checkpoint by ID
        [HttpGet("checkpoints/{id:int}")]
        public ActionResult<TourCheckpointDto> GetCheckpointById(int id)
        {
            var result = _tourService.GetCheckpoint(id);
            return CreateResponse(result);
        }

        // Route to update checkpoint
        [HttpPut("checkpoints/{id:int}")]
        public ActionResult<TourCheckpointDto> UpdateCheckpoint(int id, [FromBody] TourCheckpointDto tourCheckpointDto)
        {
            var result = _tourService.UpdateCheckpoint(id, tourCheckpointDto);
            return CreateResponse(result);
        }

        // Route to delete checkpoint
        [HttpDelete("checkpoints/{id:int}")]
        public ActionResult DeleteCheckpoint(int id)
        {
            var result = _tourService.DeleteCheckpoint(id);
            return CreateResponse(result);
        }


        [HttpGet("{id:int}/reviews")]
        public ActionResult<PagedResult<TourReviewDto>> GetAllReviews(int id, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourService.GetPagedReviews(id, page, pageSize);
            return CreateResponse(result);
        }

        [HttpPost("{id:int}/reviews")]
        public ActionResult<TourReviewDto> AddReview(int id, [FromBody] TourReviewDto reviewDto)
        {
            var result = _tourService.AddReview(id, reviewDto);
            return CreateResponse(result);
        }

        [HttpPut("reviews/{reviewId:int}")]
        public ActionResult<TourReviewDto> UpdateReview(int reviewId, [FromBody] TourReviewDto reviewDto)
        {
            var result = _tourService.UpdateReview(reviewId, reviewDto);
            return CreateResponse(result);
        }

        [HttpDelete("reviews/{reviewId:int}")]
        public ActionResult DeleteReview(int reviewId)
        {
            var result = _tourService.DeleteReview(reviewId);
            return CreateResponse(result);
        }

        [HttpPost("{id:int}/archive")]
        public ActionResult ArchiveTour(int id)
        {
            var result = _tourService.ArchiveTour(id);
            return CreateResponse(result);
        }
        [HttpPost("{id:int}/publish")]
        public ActionResult PublishTour(int id)
        {
            var result = _tourService.PublishTour(id);
            return CreateResponse(result);
        }

        
    }
}
