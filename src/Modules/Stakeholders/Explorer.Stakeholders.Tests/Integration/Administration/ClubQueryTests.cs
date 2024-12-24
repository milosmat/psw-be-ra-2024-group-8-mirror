using Explorer.API.Controllers.Tourist;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Tests.Integration.Administration
{
    public class ClubQueryTests : BaseStakeholdersIntegrationTest
    {
        public ClubQueryTests(StakeholdersTestFactory factory) : base(factory) { }

        [Theory]
        [InlineData(0, 0, 3, 3)]
        [InlineData(1, 1, 1, 3)] // Testira stranicu sa 1 rezultat po stranici
        public void Retrieves_all(int pageNumber, int pageSize, int expectedResultsCount, int expectedTotalCount)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = ((ObjectResult)controller.GetAll(pageNumber, pageSize).Result)?.Value as PagedResult<ClubDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(expectedResultsCount);
            result.TotalCount.ShouldBe(expectedTotalCount);
        }

        private static ClubController CreateController(IServiceScope scope)
        {
            return new ClubController(scope.ServiceProvider.GetRequiredService<IClubService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
