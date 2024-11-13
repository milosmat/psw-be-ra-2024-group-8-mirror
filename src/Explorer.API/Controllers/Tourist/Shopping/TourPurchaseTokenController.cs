using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.Shopping
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/tokens")]
    public class TourPurchaseTokenController : BaseApiController
    {
        private readonly ITourPurchaseTokenService _tokenService;

        public TourPurchaseTokenController(ITourPurchaseTokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet]
        public ActionResult<PagedResult<TourPurchaseTokenDTO>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tokenService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpGet("{id}")]

        public ActionResult<TourPurchaseTokenDTO> GetById(int id)
        {
            var result = _tokenService.Get(id);
            return CreateResponse(result);
        }

        [HttpPost]
        public ActionResult<TourPurchaseTokenDTO> Create([FromBody] TourPurchaseTokenDTO tokenDto)
        {
            var currentUserId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            tokenDto.TouristId = currentUserId;
            tokenDto.CreatedDate = DateTime.UtcNow;
            tokenDto.ExpiredDate = tokenDto.CreatedDate.AddYears(1);

            var result = _tokenService.Create(tokenDto);
            return CreateResponse(result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<TourPurchaseTokenDTO> Update([FromBody] TourPurchaseTokenDTO tokenDto)
        {
            var currentUserId = int.Parse(User.Claims.First(c => c.Type == "id").Value);

            if (tokenDto.TouristId != currentUserId)
            {
                return Forbid("You are not the owner of this shopping cart.");
            }

            var result = _tokenService.Update(tokenDto);
            return CreateResponse(result);
        }


        [HttpDelete("{id}")]

        public ActionResult Delete(int id)
        {
            var currentUser = int.Parse(User.Claims.First(c => c.Type == "id").Value);

            var result = _tokenService.Get(id);

            if (result.IsFailed)
            {
                return NotFound(result.Errors?.FirstOrDefault()?.Message);
            }

            var existingToken = result.Value;

            if (existingToken.TouristId != currentUser)
            {
                return Forbid();
            }

            var DeleteResult = _tokenService.Delete(id);
            return CreateResponse(DeleteResult);
        }


        // Nova akcija za vraćanje svih kupljenih tura za korisnika
        [HttpGet("purchased-tours")]
        public ActionResult<List<TourDTO>> GetPurchasedTours()
        {
            var currentUserId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var result = _tokenService.GetPurchasedToursByTouristId(currentUserId);

            if (result.IsFailed)
            {
                return NotFound(result.Errors?.FirstOrDefault()?.Message);
            }

            return Ok(result.Value);
        }
    }
}
