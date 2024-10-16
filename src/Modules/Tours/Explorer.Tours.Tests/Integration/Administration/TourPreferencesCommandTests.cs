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
                TransportPreferences = new Dictionary<TransportMode, int>()
                {
                    { TransportMode.WALK, 2 },
                    { TransportMode.BIKE, 3 },
                    { TransportMode.CAR, 1 }
                },
                InterestTags = new List<string>() { "nature" }

            };

            // Act
            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TourPreferencesDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.Difficulty.ShouldBe(newEntity.Difficulty);
            result.TransportPreferences.ShouldBe(newEntity.TransportPreferences);
            result.InterestTags.ShouldBe(newEntity.InterestTags);
        }

        [Fact]
        public void Create_fails_invalid_data()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new TourPreferencesDto
            {
                InterestTags = new List<string>() { "Test" }
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
                Id = 1,
                Difficulty = DifficultyLevel.EASY,
                TransportPreferences = new Dictionary<TransportMode, int>()
                {
                    { TransportMode.WALK, 2 },
                    { TransportMode.BIKE, 3 },
                    { TransportMode.CAR, 1 }
                },
                InterestTags = new List<string>() { "beach" }
            };

            // Act
            var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as TourPreferencesDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(-1);
            result.Difficulty.ShouldBe(updatedEntity.Difficulty);
            result.TransportPreferences.ShouldBe(updatedEntity.TransportPreferences);
            result.InterestTags.ShouldBe(updatedEntity.InterestTags);

            // Assert - Database
            var storedEntity = dbContext.TourPreferences.FirstOrDefault(i => i.Id == updatedEntity.Id); // Koristi ID da pronađeš ažurirani entitet
            storedEntity.ShouldNotBeNull();
            storedEntity.TransportPreferences.ShouldBeEquivalentTo(updatedEntity.TransportPreferences); // Proverava TransportPreferences (dictionary)
            storedEntity.InterestTags.ShouldBeEquivalentTo(updatedEntity.InterestTags); // Proverava InterestTags (listu)

            var oldEntity = dbContext.TourPreferences.FirstOrDefault(i => i.Id != updatedEntity.Id); // Proveri da li postoji entitet sa drugačijim ID-om
            oldEntity.ShouldBeNull(); // Ako očekuješ da stari entitet više ne postoji
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
            var result = (OkResult)controller.Delete(3);

            // Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);

            // Assert - Database
            var storedCourse = dbContext.Equipment.FirstOrDefault(i => i.Id == 3);
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
