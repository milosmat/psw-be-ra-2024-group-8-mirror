using Explorer.API.Controllers.Tourist;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Infrastructure.Database;
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
    public class VisitedCheckpointCommandTests : BaseToursIntegrationTest
    {
        public VisitedCheckpointCommandTests(ToursTestFactory factory) : base(factory) { }

        [Fact]
        public void Creates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newEntity = new VisitedCheckpointDTO
            {
                CheckpointId = 1,
                VisitTime = DateTime.UtcNow,
                Secret = "SecretMessage123"
            };

            // Act
            //var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as VisitedCheckpointDTO;

            // Assert - Response
            //result.ShouldNotBeNull();
            //result.CheckpointId.ShouldBe(newEntity.CheckpointId);
            //result.VisitTime.ShouldBe(newEntity.VisitTime);
            //result.Secret.ShouldBe(newEntity.Secret);

            // Assert - Database
            var storedEntity = dbContext.VisitedCheckpoints.FirstOrDefault(i => i.CheckpointId == newEntity.CheckpointId);
            storedEntity.ShouldNotBeNull();
            storedEntity.Secret.ShouldBe(newEntity.Secret);
        }

        [Fact]
        public void Create_fails_invalid_data()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var newEntity = new VisitedCheckpointDTO
            {
                VisitTime = DateTime.UtcNow
                // Missing CheckpointId and Secret
            };

            // Act
           // var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TourDTO;


            // Assert
            //result.ShouldNotBeNull();
            //result.StatusCode.ShouldBe(400);
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
