using Explorer.API.Controllers.Administrator.Administration;
using Explorer.API.Controllers.Tourist;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Tests.Integration.Administration
{
    [Collection("Sequential")]
    public class TourPreferencesCommandTests : BaseToursIntegrationTest
    {
        public TourPreferencesCommandTests(ToursTestFactory factory) : base(factory) { }

        [Fact]
        public void Creates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newEntity = new TourPreferencesDto
            {
                Difficulty = DifficultyLevel.EASY,
                WalkRating = 3,
                BikeRating = 2,
                CarRating = 0,
                BoatRating = 1,
                InterestTags = new List<string>() { "nature" }

            };

            // Act
            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TourPreferencesDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.Difficulty.ShouldBe(newEntity.Difficulty);
            result.WalkRating.ShouldBe(newEntity.WalkRating);
            result.BikeRating.ShouldBe(newEntity.BikeRating);
            result.CarRating.ShouldBe(newEntity.CarRating);
            result.BoatRating.ShouldBe(newEntity.BoatRating);
            result.InterestTags.ShouldBe(newEntity.InterestTags);

            // Assert - Database
            var storedEntity = dbContext.TourPreferences.FirstOrDefault(i => i.WalkRating == newEntity.WalkRating);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldBe(result.Id);
        }

        [Fact]
        public void Create_fails_invalid_data()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new TourPreferencesDto
            {
                WalkRating = 4
            };

            // Act
            var result = (ObjectResult)controller.Create(updatedEntity).Result;

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
            var updatedEntity = new TourPreferencesDto
            {
                Id = -1,
                Difficulty = DifficultyLevel.EASY,
                WalkRating = 0,
                BikeRating = 2,
                CarRating = 2,
                BoatRating = 2,
                InterestTags = new List<string>() { "rucnoDodat" }
            };

            // Act
            var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as TourPreferencesDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(-1);
            result.Difficulty.ShouldBe(updatedEntity.Difficulty);
            result.WalkRating.ShouldBe(updatedEntity.WalkRating);   
            result.BikeRating.ShouldBe(updatedEntity.BikeRating);
            result.CarRating.ShouldBe(updatedEntity.CarRating);
            result.BoatRating.ShouldBe(updatedEntity.BoatRating);
            result.InterestTags.ShouldBe(updatedEntity.InterestTags);

            // Assert - Database
            var storedEntity = dbContext.TourPreferences.FirstOrDefault(i => i.WalkRating == 0); 
            storedEntity.ShouldNotBeNull();
            storedEntity.InterestTags.ShouldBeEquivalentTo(updatedEntity.InterestTags);
            result.Difficulty.ShouldBe(updatedEntity.Difficulty);
            result.BikeRating.ShouldBe(updatedEntity.BikeRating);
            result.CarRating.ShouldBe(updatedEntity.CarRating);
            result.BoatRating.ShouldBe(updatedEntity.BoatRating);
            var oldEntity = dbContext.TourPreferences.FirstOrDefault(i => i.WalkRating == 2); 
            oldEntity.ShouldBeNull(); 
        }

        [Fact]
        public void Update_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new TourPreferencesDto
            {
                Id = -1000,
                InterestTags = new List<string>() { "Test" }
            };

            // Act
            var result = (ObjectResult)controller.Update(updatedEntity).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }

        [Fact]
        public void Deletes()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            // Act
            var result = (OkResult)controller.Delete(-1);

            // Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);

            // Assert - Database
            var storedCourse = dbContext.TourPreferences.FirstOrDefault(i => i.Id == -1);
            storedCourse.ShouldBeNull();
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
        private static TourPreferencesController CreateController(IServiceScope scope)
        {
            return new TourPreferencesController(scope.ServiceProvider.GetRequiredService<ITourPreferenceService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
