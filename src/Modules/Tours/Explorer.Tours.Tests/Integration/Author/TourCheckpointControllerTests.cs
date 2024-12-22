using Explorer.API.Controllers.Author;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.API.Public.Author;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.ValueObjects;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
            var tourController = CreateTourController(scope);
            var tourDbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newTour = new TourDTO
            {
                Name = "Nova Tura",
                Description = "Opis nove ture.",
                Weight = "medium",
                Tags = new[] { "avantura", "priroda" },
                Price = 100,
                Status = 1,
                LengthInKm = 15,
                PublishedDate = DateTime.UtcNow,
                ArchivedDate = DateTime.UtcNow,
                AuthorId = 1

            };

            // Act
            var createResult = ((ObjectResult)tourController.Create(newTour).Result)?.Value as TourDTO;

            var newCheckpoint = new TourCheckpointDto
            {
               
                Latitude = 45.2671,
                Longitude = 19.8335,
                CheckpointName = "Novi Sad",
                CheckpointDescription = "Glavni grad Vojvodine.",
                Image = "url_to_image",
                TourId = createResult.Id
            };

            // Act
            var result = ((ObjectResult)controller.Create(newCheckpoint).Result)?.Value as TourCheckpointDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);  // ID should be auto-generated and not 0
            result.CheckpointName.ShouldBe(newCheckpoint.CheckpointName);

            // Assert - Database
            // Use the ID from the result instead of newCheckpoint.Id
            var storedCheckpoint = dbContext.TourCheckpoints.FirstOrDefault(i => i.Id == result.Id);
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
            var tourController = CreateTourController(scope);
            var tourDbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newTour = new TourDTO
            {
                Name = "Nova Tura",
                Description = "Opis nove ture.",
                Weight = "medium",
                Tags = new[] { "avantura", "priroda" },
                Price = 100,
                Status = 1,
                LengthInKm = 15,
                PublishedDate = DateTime.UtcNow,
                ArchivedDate = DateTime.UtcNow,
                AuthorId = 1

            };

            // Act
            var createResult = ((ObjectResult)tourController.Create(newTour).Result)?.Value as TourDTO;


            var newCheckpoint = new TourCheckpointDto
            {

                Latitude = 45.2671,
                Longitude = 19.8335,
                CheckpointName = "Novi Sad",
                CheckpointDescription = "Glavni grad Vojvodine.",
                Image = "url_to_image",
                TourId = 1
            };

            // Act
            var result = ((ObjectResult)controller.Create(newCheckpoint).Result)?.Value as TourCheckpointDto;

            var updatedCheckpoint = new TourCheckpointDto
            {
                Id = result.Id,
                Latitude = 45.2671,
                Longitude = 19.8335,
                CheckpointName = "Novi Sad",
                CheckpointDescription = "Glavni grad Vojvodine.",
                Image = "url_to_image",
                TourId = 1
            };
            var existingEntity = dbContext.ChangeTracker
            .Entries<TourCheckpoint>()
            .FirstOrDefault(e => e.Entity.Id == updatedCheckpoint.Id);
            if (existingEntity != null)
            {
                dbContext.Entry(existingEntity.Entity).State = EntityState.Detached;
            }
            // Act
            var updateResult = ((ObjectResult)controller.Update(updatedCheckpoint).Result)?.Value as TourCheckpointDto;

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(updateResult.Id);
            result.CheckpointName.ShouldBe(updateResult.CheckpointName);
            result.CheckpointDescription.ShouldBe(updateResult.CheckpointDescription);

            var storedCheckpoint = dbContext.TourCheckpoints.FirstOrDefault(i => i.Id == updateResult.Id);
            storedCheckpoint.ShouldNotBeNull();
            storedCheckpoint.CheckpointDescription.ShouldBe(updateResult.CheckpointDescription);
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
            var tourController = CreateTourController(scope);
            var tourDbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newTour = new TourDTO
            {
                Name = "Nova Tura",
                Description = "Opis nove ture.",
                Weight = "medium",
                Tags = new[] { "avantura", "priroda" },
                Price = 100,
                Status = 1,
                LengthInKm = 15,
                PublishedDate = DateTime.UtcNow,
                ArchivedDate = DateTime.UtcNow,
                AuthorId = 1

            };

            // Act
            var createTourResult = ((ObjectResult)tourController.Create(newTour).Result)?.Value as TourDTO;
            var newCheckpoint = new TourCheckpointDto
            {
                
                Latitude = 45.2671,
                Longitude = 19.8335,
                CheckpointName = "Izmenjeni Beograd",
                CheckpointDescription = "Izmenjen opis glavnog grada Srbije.",
                Image = "url_to_updated_image",
                TourId = createTourResult.Id
            };
            var createResult = ((ObjectResult)controller.Create(newCheckpoint).Result)?.Value as TourCheckpointDto;

            var result = (OkResult)controller.Delete(createResult.Id);
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);

            var storedCheckpoint = dbContext.TourCheckpoints.FirstOrDefault(i => i.Id == createResult.Id);
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
        private static TourController CreateTourController(IServiceScope scope)
        {
            return new TourController(scope.ServiceProvider.GetRequiredService<ITourService>(), scope.ServiceProvider.GetRequiredService<IEquipmentService>(), scope.ServiceProvider.GetRequiredService<ITourCheckpointService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
