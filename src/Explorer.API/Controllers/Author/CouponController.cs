using Explorer.BuildingBlocks.Core.UseCases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Explorer.Payments.API.Public.Tourist;
using Explorer.Payments.API.Dtos;
using Explorer.Games.API.Public.Tourist;
using Explorer.Games.Core.UseCases.Tourist;
using FluentResults;
using Explorer.Stakeholders.Core.Domain;


namespace Explorer.API.Controllers.Author
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/author/coupon")]
    public class CouponController : BaseApiController
    {
        private readonly ICouponService _couponService;
        private readonly IEmailService _emailService;
        public CouponController(ICouponService couponService, IEmailService emailService)
        {
            _couponService = couponService;
            _emailService = emailService;
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

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> PublishCoupon(int id)
        {
            var result = _couponService.MakeCouponPublic((long)id);
            if (result.IsFailed)
            {
                return StatusCode(500, new { message = "An internal error occurred while processing your request. Please try later." });
            }
            var coupon = result.Value;
            string subject = "The New Coupon Available";
            string body = $@"
                        <!DOCTYPE html>
                        <html>
                        <head>
                            <meta charset=""utf-8"">
                            <title>The New Coupon Available</title>
                        </head>
                        <body>
                            <div style=""font-family: Arial, sans-serif; max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 8px; background-color: #f9f9f9; text-align: center;"">
                                <p class=""header"" style=""font-size: 24px; font-weight: bold; color: #2c3e50;""> Your Exclusive Discount is Here! </p>
                                <p style=""font-size: 16px;"">Enjoy a special <strong>{coupon.DiscountPercentage}%</strong> discount on your next trip!</p>
                                <p><strong>Coupon Code:</strong> <span class=""code"" style=""font-size: 22px; font-weight: bold; color: #27ae60;"">{coupon.Code}</span></p>
                                <p><strong>Valid Until:</strong> {coupon.ExpiryDate:dd.MM.yyyy}</p>
                                <p style=""font-size: 16px;"">Don't miss out – book your next adventure now!</p>
                                <a href=""http://localhost:4200/"" style=""display: inline-block; padding: 12px 20px; margin-top: 15px; background: #27ae60; color: white; text-decoration: none; font-size: 16px; font-weight: bold; border-radius: 5px;"">Visit Our App 🌐</a>
                                <p class=""footer"" style=""margin-top: 20px; font-size: 12px; color: #7f8c8d;"">* This coupon is valid for a limited time only.</p>
                            </div>
                        </body>
                        </html>";


            var emailResult = await _emailService.SendNewCouponNotificatinToAllTourists(subject, body);

            if (emailResult.IsFailed)
            {
                return StatusCode(500, new { message = "Coupon published, but failed to send email notifications." });
            }

            return Ok(new {message = "Coupon is published successfully."});
        }
    }
}
