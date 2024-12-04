using Explorer.API.Controllers.Tourist;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Tests.Integration.Tourist
{
    public class VisitedCheckpointQueryTests : BaseToursIntegrationTest
    {
        public VisitedCheckpointQueryTests(ToursTestFactory factory) : base(factory) { }

        [Fact]
        public void Retrieves_all()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            //var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as PagedResult<VisitedCheckpointDTO>;

            // Assert
            //result.ShouldNotBeNull();
            //result.Results.Count.ShouldBe(3);  // assuming we have 3 records
            //result.TotalCount.ShouldBe(3);  // assuming we have 3 records
        }

        private static VisitedCheckpointController CreateController(IServiceScope scope)
        {
            return new VisitedCheckpointController(scope.ServiceProvider.GetRequiredService<IVisitedCheckpointService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }

}
