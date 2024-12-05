using Explorer.API.Controllers.Author;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public.Tourist;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using static Explorer.Payments.API.Dtos.ShoppingCartDTO;

namespace Explorer.API.Controllers.Tourist.Shopping
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/shoppingcart")]
    public class ShoppingCardController : BaseApiController
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ICouponService _couponService;

        public ShoppingCardController(IShoppingCartService shoppingCartService, ICouponService couponService)
        {
            _shoppingCartService = shoppingCartService;
            _couponService = couponService;
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

        [HttpPost("addboundle/{touristId}")]
        public ActionResult AddBoundleToCart(long touristId, [FromBody] ShoppingBundleDto shoppingBoundleDto)
        {
            // Pozivate servis sa pravim turističkim ID i DTO podacima
            _shoppingCartService.AddBoundleToCart(touristId, shoppingBoundleDto);

            return Ok(new { message = "Pacage added to cart successfully" });
        }

        [HttpDelete("remove-bundle/{touristId}/{bundleId}")]
        public ActionResult RemoveBundleFromCart(long touristId, long bundleId)
        {
            _shoppingCartService.RemoveBundleFromCart(touristId, bundleId);
            return Ok(new { message = "Bundle removed from cart successfully" });
        }

        [HttpGet("prni/{touristId}")]
        public IActionResult GetBundles(long touristId)
        {
            var result = _shoppingCartService.GetBundlesForTourist(touristId);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors.FirstOrDefault()?.Message);
            }

            return Ok(result.Value);
        }

        [HttpPost("{touristId:int}/apply-coupon")]
        public ActionResult<ShoppingCartDTO> ApplyCoupon(int touristId, [FromQuery] string couponCode)
        {
            if (string.IsNullOrEmpty(couponCode))
            {
                return BadRequest("Coupon code is required.");
            }


            var shoppingCart = _shoppingCartService.GetShoppingCart(touristId);
            if (shoppingCart == null)
            {
                return NotFound("Shopping cart not found.");
            }

            var updatedCartItems = _couponService.ApplyCouponOnCartItems(couponCode, shoppingCart.ShopingItems);
            shoppingCart.ShopingItems = updatedCartItems;
            _shoppingCartService.Update(shoppingCart);  // Pretpostavljam da imate metodu za ažuriranje korpe u bazi

            return Ok(shoppingCart);
        }


    }
}
