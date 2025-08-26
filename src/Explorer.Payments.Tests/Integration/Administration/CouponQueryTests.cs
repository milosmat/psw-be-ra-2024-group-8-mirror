using Explorer.API.Controllers.Author;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public.Tourist;
using Explorer.Payments.Infrastructure.Database;
using Explorer.Stakeholders.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class CouponQueryTests : BasePaymentsIntegrationTest
    {
        public CouponQueryTests(PaymentsTestFactory factory) : base(factory) { }

        [Theory]
        [InlineData(-1, "code1", 20, 1, true)] 
        [InlineData(-2, "code2", 25, 1, false)] 
        public void Retrieves_all(int id, string couponCode, int discountPercentage, int authorId, bool isPublic)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
            var controller = CreateController(scope);

            DateTime expiryDate = DateTime.UtcNow.AddDays(20);

            var sqlScript = @"
                               INSERT INTO payments.""Coupons""(
	                            ""Id"", ""Code"", ""DiscountPercentage"", ""ExpiryDate"", ""TourId"", ""AuthorId"", ""RecipientId"", ""IsPublic"")
	                            VALUES ({0}, {1}, {2}, {3}, null, {4}, null, {5});";   

            dbContext.Database.ExecuteSqlRaw(sqlScript, id, couponCode, discountPercentage, expiryDate, authorId, isPublic);

            // Act
            var result = ((ObjectResult)controller.GetAll(0, 5).Result)?.Value as PagedResult<CouponDTO>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(dbContext.Coupons.Count());
            result.TotalCount.ShouldBe(dbContext.Coupons.Count());
        }
        private static CouponController CreateController(IServiceScope scope)
        {
            return new CouponController(scope.ServiceProvider.GetRequiredService<ICouponService>(),
                                        scope.ServiceProvider.GetRequiredService<IEmailService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }

    }


}
