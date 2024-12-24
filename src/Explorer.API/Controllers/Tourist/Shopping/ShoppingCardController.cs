using Explorer.API.Controllers.Author;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public.Tourist;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public ActionResult<ShoppingCartItemDto> AddTourToCart(long touristId, [FromBody] ShoppingCartItemDto shoppingCartItemDto)
        {
            try
            {
                List<ShoppingCartItemDto> results = _shoppingCartService.AddTourToCart(touristId, shoppingCartItemDto);

                var addedItem = results.FirstOrDefault(item => item.TourId == shoppingCartItemDto.TourId);
                if(addedItem == null)
                {
                    return BadRequest(new { message = "Faild to add new tour to the cart." });
                }
               
                return CreatedAtAction(nameof(AddTourToCart), new { id = addedItem.Id }, addedItem);
                
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest("Failed to update the database.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the tour to the cart.", error = ex.Message });
            }
        }


        [HttpDelete("remove/{touristId}/{tourId}")]
        public ActionResult RemoveTourFromCart(long touristId, long tourId)
        {
            try
            {
                var success = _shoppingCartService.RemoveTourFromCart(touristId, tourId);

                if (!success)
                {
                    var shoppingCart = _shoppingCartService.GetShoppingCart(touristId);
                    if (shoppingCart == null)
                    {
                        return NotFound(new { message = $"Shopping cart not found." });
                    }

                    var itemToRemove = shoppingCart.ShopingItems.FirstOrDefault(item => item.TourId == tourId);
                    if (itemToRemove == null)
                    {
                        return NotFound(new { message = $"The order item you want to remove from cart not found." });
                    }
                    return BadRequest(new { message = "Failed to remove the order item from the cart due to an issue while updating the database." });
                }
                return Ok(new { message = "Order item removed from cart successfully" });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
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
            _shoppingCartService.Update(shoppingCart);  

            return Ok(shoppingCart);
        }


    }
}
