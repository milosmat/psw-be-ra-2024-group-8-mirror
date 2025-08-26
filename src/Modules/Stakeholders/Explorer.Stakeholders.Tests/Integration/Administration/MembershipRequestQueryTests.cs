using Explorer.API.Controllers.Tourist;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Tests.Integration.Administration
{
    public class MembershipRequestQueryTests : BaseStakeholdersIntegrationTest
    {
        public MembershipRequestQueryTests(StakeholdersTestFactory factory) : base(factory) { }

        [Theory]
        [InlineData(0, 3, 1)]  // Prva stranica sa 3 komentara
        public void Retrieves_all(int pageNumber, int pageSize, int expectedResultsCount)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = ((ObjectResult)controller.GetAll(-1, pageNumber, pageSize).Result)?.Value as PagedResult<MembershipRequestDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(expectedResultsCount);
            result.TotalCount.ShouldBeGreaterThanOrEqualTo(expectedResultsCount);
        }
        private static MembershipRequestContoller CreateController(IServiceScope scope)
        {
            return new MembershipRequestContoller(scope.ServiceProvider.GetRequiredService<IMembershipRequestService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }


    }
}
