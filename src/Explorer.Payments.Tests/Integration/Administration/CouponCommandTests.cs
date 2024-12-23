using Explorer.API.Controllers.Author;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public.Tourist;
using Explorer.Payments.Infrastructure.Database;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Tests.Integration.Administration
{
    [Collection("Sequential")]
    public class CouponCommandTests : BasePaymentsIntegrationTest
    {
        public CouponCommandTests(PaymentsTestFactory factory) : base(factory) { }

        [Theory]
        [InlineData("ABC12345", 10, -1, -11)] // Valid coupon with an expiry date
        [InlineData("XYZ98765", 20, -2, -12)]           // Valid coupon without an expiry date
        public void CreatesCoupon(string code, int discountPercentage, int tourId, int authorId)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope); // Method to create an instance of the CouponController
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

            var newCoupon = new CouponDTO
            {
                Code = code,
                DiscountPercentage = discountPercentage,
                ExpiryDate = DateTime.UtcNow.AddYears(1), // Expiry date one year from now
                TourId = tourId,
                AuthorId = authorId
            };

            // Act
            var result = ((ObjectResult)controller.Create(newCoupon).Result)?.Value as CouponDTO;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Code.ShouldBe(newCoupon.Code);
            result.DiscountPercentage.ShouldBe(newCoupon.DiscountPercentage);
            result.ExpiryDate.ShouldBe(newCoupon.ExpiryDate);
            result.TourId.ShouldBe(newCoupon.TourId);
            result.AuthorId.ShouldBe(newCoupon.AuthorId);

            // Assert - Database
            var storedCoupon = dbContext.Coupons.FirstOrDefault(c => c.Code == newCoupon.Code);
            storedCoupon.ShouldNotBeNull();
            storedCoupon.Code.ShouldBe(newCoupon.Code);
            storedCoupon.DiscountPercentage.ShouldBe(newCoupon.DiscountPercentage);
            storedCoupon.ExpiryDate.ShouldBe(newCoupon.ExpiryDate);
            storedCoupon.TourId.ShouldBe(newCoupon.TourId);
            storedCoupon.AuthorId.ShouldBe(newCoupon.AuthorId);
        }

        [Theory]
        [InlineData(null, 10, 1, 500)] // Invalid Code
        [InlineData("VALIDCODE", -5, 1, 500)] // Invalid DiscountPercentage
        public void CreateCoupon_Fails_InvalidData(string code, int discountPercentage, long authorId, int expectedStatusCode)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var newCoupon = new CouponDTO
            {
                Code = code,
                DiscountPercentage = discountPercentage,
                ExpiryDate = DateTime.UtcNow.AddYears(1), // Expiry date one year from now
                AuthorId = authorId
            };

            // Act
            var result = (ObjectResult)controller.Create(newCoupon).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(expectedStatusCode);
        }

        [Theory]
        [InlineData(-2, "UpdatedCode", 25, -13)]
        public void Updates(int id, string code, int percent, int authorId)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

            var updatedCoupon = new CouponDTO
            {
                Id = id,
                Code = code,
                DiscountPercentage = percent,
                ExpiryDate = DateTime.UtcNow.AddYears(2), // Expiry date one year from now
            };

            // Act
            var result = ((ObjectResult)controller.Update(updatedCoupon).Result)?.Value as CouponDTO;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(id);
            result.Code.ShouldBe(updatedCoupon.Code);
            result.DiscountPercentage.ShouldBe(updatedCoupon.DiscountPercentage);

            // Assert - Database
            var storedCoupon = dbContext.Coupons.FirstOrDefault(i => i.Id == id);
            storedCoupon.ShouldNotBeNull();
            storedCoupon.Code.ShouldBe(code);
            storedCoupon.DiscountPercentage.ShouldBe(percent);
        }


         [Theory]
        [InlineData(-1000, "UpdatedCodeMistake", 25, -13)]
        public void Update_fails_invalid_id(int id, string code, int percent, int authorId)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

            var updatedCoupon = new CouponDTO
            {
                Id = id,
                Code = code,
                DiscountPercentage = percent,
                ExpiryDate = DateTime.UtcNow.AddYears(2), // Expiry date one year from now
            };

            // Act
            var result = (ObjectResult)controller.Update(updatedCoupon).Result;

            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(500);
        }

        [Theory]
        [InlineData("ACDELETE", 20, -1, -11,true)] // Valid coupon with an expiry date
        public void Deletes(string code, int discountPercentage, int tourId, int authorId, bool shouldExist)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

            var newCoupon = new CouponDTO
            {
                Code = code,
                DiscountPercentage = discountPercentage,
                ExpiryDate = DateTime.UtcNow.AddYears(1), // Expiry date one year from now
                TourId = tourId,
                AuthorId = authorId
            };
            var createdCoupon = ((ObjectResult)controller.Create(newCoupon).Result)?.Value as CouponDTO;
            var createdCouponId = createdCoupon?.Id ?? -1;

            // Pre-check: Verify the initial existence state based on shouldExist
            var preDeleteEntity = dbContext.Coupons.FirstOrDefault(b => b.Id == createdCouponId);
            if (shouldExist)
            {
                preDeleteEntity.ShouldNotBeNull();
            }
            else
            {
                preDeleteEntity.ShouldBeNull();
            }

            // Act: Call Delete method
            controller.Delete(createdCouponId);

            // Assert - Check if entity was deleted from the database
            var postDeleteEntity = dbContext.Coupons.FirstOrDefault(i => i.Id == createdCouponId);
            postDeleteEntity.ShouldBeNull(); // Should be null if successfully deleted or if it didn't exist initially
        }

        [Theory]
        [InlineData(-1000, 404)]
        public void Delete_fails_invalid_id(int id, int expectedStatusCode)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = (ObjectResult)controller.Delete(id);

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(expectedStatusCode);
        }


        private static CouponController CreateController(IServiceScope scope)
        {
            return new CouponController(scope.ServiceProvider.GetRequiredService<ICouponService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }

    }
}
