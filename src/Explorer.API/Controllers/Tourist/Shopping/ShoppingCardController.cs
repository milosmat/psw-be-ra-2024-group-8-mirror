using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public.Tourist;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Explorer.Payments.API.Dtos.ShoppingCartDTO;

namespace Explorer.API.Controllers.Tourist.Shopping
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/shoppingcart")]
    public class ShoppingCardController : BaseApiController
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCardController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet("{touristId}")]
        public ActionResult<ShoppingCartDTO> GetShoppingCart(long touristId)
        {
            var shoppingCart = _shoppingCartService.GetShoppingCart(touristId);
            if (shoppingCart == null)
            {
                return NotFound(new { message = "Shopping cart not found" });
            }
            return Ok(shoppingCart);
        }

        [HttpPost("add/{touristId}")]
        public ActionResult AddTourToCart(long touristId, [FromBody] ShoppingCartItemDTO shoppingCartItemDto)
        {
            // Pozivate servis sa pravim turističkim ID i DTO podacima
            _shoppingCartService.AddTourToCart(touristId, shoppingCartItemDto);

            return Ok(new { message = "Tour added to cart successfully" });
        }


        [HttpDelete("remove/{touristId}/{tourId}")]
        public ActionResult RemoveTourFromCart(long touristId, long tourId)
        {
            _shoppingCartService.RemoveTourFromCart(touristId, tourId);
            return Ok(new { message = "Tour removed from cart successfully" });
        }

        [HttpPost("checkout/{touristId}")]
        public ActionResult Checkout(long touristId)
        {
            var result = _shoppingCartService.Checkout(touristId);
            if (result.IsFailed)
            {
                return BadRequest(new { message = result.Errors.First().Message });
            }
            return Ok(new { message = "Checkout successful, tokens created." });
        }


    }
}
