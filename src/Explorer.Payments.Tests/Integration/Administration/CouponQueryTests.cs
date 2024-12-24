using Explorer.API.Controllers.Author;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public.Tourist;
using Explorer.Stakeholders.API.Dtos;
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
    public class CouponQueryTests : BasePaymentsIntegrationTest
    {
        public CouponQueryTests(PaymentsTestFactory factory) : base(factory) { }

        [Theory]
        [InlineData(0, 3, 3)]  // Prva stranica sa 3 komentara
        public void Retrieves_all(int pageNumber, int pageSize, int expectedResultsCount)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = ((ObjectResult)controller.GetAll(pageNumber, pageSize).Result)?.Value as PagedResult<CouponDTO>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(expectedResultsCount);
            result.TotalCount.ShouldBeGreaterThanOrEqualTo(expectedResultsCount);
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
