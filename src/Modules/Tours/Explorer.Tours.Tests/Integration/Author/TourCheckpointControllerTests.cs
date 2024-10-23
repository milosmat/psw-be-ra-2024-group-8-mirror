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
    public class TourCheckpointControllerTests : BaseToursIntegrationTest
    {
        public TourCheckpointControllerTests(ToursTestFactory factory) : base(factory)
        {
        }

        [Fact]
        public void Creates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newCheckpoint = new TourCheckpointDto
            {
                Id = 0, // This ID will be ignored and auto-generated in the database
                Latitude = 45.2671,
                Longitude = 19.8335,
                CheckpointName = "Novi Sad",
                CheckpointDescription = "Glavni grad Vojvodine.",
                Image = "url_to_image"
            };

            // Act
            var result = ((ObjectResult)controller.Create(newCheckpoint).Result)?.Value as TourCheckpointDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);  // ID should be auto-generated and not 0
            result.CheckpointName.ShouldBe(newCheckpoint.CheckpointName);

            // Assert - Database
            // Use the ID from the result instead of newCheckpoint.Id
            var storedCheckpoint = dbContext.TourCheckpoint.FirstOrDefault(i => i.Id == result.Id);
            storedCheckpoint.ShouldNotBeNull();  // Ensure the checkpoint is found
            storedCheckpoint.CheckpointName.ShouldBe(result.CheckpointName);  // Ensure the data matches
        }


        [Fact]
        public void Create_fails_invalid_data()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var invalidCheckpoint = new TourCheckpointDto
            {
                CheckpointDescription = "Opis bez imena"
            };

            // Act
            var result = (ObjectResult)controller.Create(invalidCheckpoint).Result;

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
            var updatedCheckpoint = new TourCheckpointDto
            {
                Id = 1,
                Latitude = 45.2671,
                Longitude = 19.8335,
                CheckpointName = "Izmenjeni Novi Sad",
                CheckpointDescription = "Izmenjen opis glavnog grada Vojvodine.",
                Image = "url_to_updated_image"
            };

            // Act
            var result = ((ObjectResult)controller.Update(updatedCheckpoint).Result)?.Value as TourCheckpointDto;

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(updatedCheckpoint.Id);
            result.CheckpointName.ShouldBe(updatedCheckpoint.CheckpointName);
            result.CheckpointDescription.ShouldBe(updatedCheckpoint.CheckpointDescription);

            var storedCheckpoint = dbContext.TourCheckpoint.FirstOrDefault(i => i.Id == updatedCheckpoint.Id);
            storedCheckpoint.ShouldNotBeNull();
            storedCheckpoint.CheckpointDescription.ShouldBe(updatedCheckpoint.CheckpointDescription);
        }

        [Fact]
        public void Update_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var invalidCheckpoint = new TourCheckpointDto
            {
                Id = -1000,
                Latitude = 45.2671,
                Longitude = 19.8335,
                CheckpointName = "Novi Sad",
                CheckpointDescription = "Glavni grad Vojvodine.",
                Image = "url_to_image"
            };

            // Act
            var result = (ObjectResult)controller.Update(invalidCheckpoint).Result;

            // Assert
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

            var storedCheckpoint = dbContext.TourCheckpoint.FirstOrDefault(i => i.Id == 1);
            storedCheckpoint.ShouldBeNull();
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

        private static TourCheckpointController CreateController(IServiceScope scope)
        {
            return new TourCheckpointController(scope.ServiceProvider.GetRequiredService<ITourCheckpointService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
