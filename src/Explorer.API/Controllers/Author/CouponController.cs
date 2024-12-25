using Explorer.BuildingBlocks.Core.UseCases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Explorer.Payments.API.Public.Tourist;
using Explorer.Payments.API.Dtos;
using Explorer.Games.API.Public.Tourist;
using Explorer.Games.Core.UseCases.Tourist;


namespace Explorer.API.Controllers.Author
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/author/coupon")]
    public class CouponController : BaseApiController
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpGet]
        public ActionResult<PagedResult<CouponDTO>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            try
            {
                var result = _couponService.GetPaged(page, pageSize);

                if (result == null)
                {
                    return NotFound("No blogs found for the specified page.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching data: {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult<CouponDTO> Create([FromBody] CouponDTO coupon)
        {
            try
            {
                CouponDTO result = _couponService.Create(coupon);

                if (result != null)
                {
                    return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
                }
                else
                {
                    return BadRequest("The coupon could not be created due to invalid data.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPut("{id:int}")]
        public ActionResult<CouponDTO> Update([FromBody] CouponDTO coupon)
        {
            try
            {
                CouponDTO result = _couponService.Update(coupon);
                return Ok(result);

            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Return a 500 error for unexpected issues
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var coupon = _couponService.Get(id);

            if (coupon == null)
            {
                return NotFound($"Blog with ID {id} not found.");
            }

            _couponService.Delete(id);

            return Ok($"Blog with ID {id} has been successfully deleted.");
        }

        [HttpPost("get-by-ids")]
        public ActionResult<IEnumerable<CouponDTO>> GetCouponsByIds([FromBody] List<long> couponIds)
        {
            try
            {
                if (couponIds == null || !couponIds.Any())
                {
                    return BadRequest("Coupon IDs cannot be null or empty.");
                }

                var coupons = _couponService.GetCouponsByIds(couponIds);

                if (coupons == null || !coupons.Any())
                {
                    return NotFound("No coupons found for the provided IDs.");
                }

                return Ok(coupons);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching coupons: {ex.Message}");
            }
        }
    }
}
