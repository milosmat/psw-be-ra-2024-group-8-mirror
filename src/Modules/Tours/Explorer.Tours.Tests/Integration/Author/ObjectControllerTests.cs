using Explorer.API.Controllers.Author;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Author;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Tests.Integration.Author
{
    [Collection("Sequential")]
    public class ObjectControllerTests : BaseToursIntegrationTest
    {
        public ObjectControllerTests(ToursTestFactory factory) : base(factory)
        {
        }

        [Fact]
        public void Creates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newObject = new ObjectDTO
            {
                Id = 0,
                Name = "new object",
                Description = "shiny new",
                Image = "url_to_image",
                Category = "Toilet"
            };

            // Act
            var result = ((ObjectResult)controller.Create(newObject).Result)?.Value as ObjectDTO;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.Name.ShouldBe(newObject.Name);

            // Assert - Database
            var storedCheckpoint = dbContext.TourCheckpoints.FirstOrDefault(i => i.CheckpointName == newObject.Name);
            storedCheckpoint.ShouldNotBeNull();
            storedCheckpoint.Id.ShouldBe(result.Id);
        }

        [Fact]
        public void Create_fails_invalid_data()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var invalidObject = new ObjectDTO
            {
                Description = "Opis bez imena" // Nedostaje Name
            };

            // Act
            var result = (ObjectResult)controller.Create(invalidObject).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(400);
        }

        [Fact]
        public void Updates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var updatedObject = new ObjectDTO
            {
                Id = 1,
                Name = "UPDATED object",
                Description = "shiny UPDATED",
                Image = "url_to_image",
                Category = "Other"
            };

            var result = ((ObjectResult)controller.Update(updatedObject).Result)?.Value as TourDTO;

            result.ShouldNotBeNull();
            result.Id.ShouldBe(updatedObject.Id);
            result.Name.ShouldBe(updatedObject.Name);
            result.Description.ShouldBe(updatedObject.Description);

            var storedTour = dbContext.Tours.FirstOrDefault(i => i.Id == updatedObject.Id);
            storedTour.ShouldNotBeNull();
            storedTour.Description.ShouldBe(updatedObject.Description);
        }

        [Fact]
        public void Update_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var invalidObject = new ObjectDTO
            {
                Id = -1000,
                Name = "Test"
            };

            var result = (ObjectResult)controller.Update(invalidObject).Result;

            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }

        [Fact]
        public void Deletes()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var result = (OkResult)controller.Delete(1);
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);

            var storedObject = dbContext.Tours.FirstOrDefault(i => i.Id == 1);
            storedObject.ShouldBeNull();
        }

        [Fact]
        public void Delete_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = (ObjectResult)controller.Delete(-1000);

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }

        private static ObjectController CreateController(IServiceScope scope)
        {
            return new ObjectController(scope.ServiceProvider.GetRequiredService<IObjectService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
