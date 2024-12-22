using Explorer.API.Controllers.Administrator.Administration;
using Explorer.API.Controllers.Tourist;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Microsoft.EntityFrameworkCore;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace Explorer.Tours.Tests.Integration.Administration
{
    [Collection("Sequential")]
    public class TourPreferencesQueryTests : BaseToursIntegrationTest
    {
        public TourPreferencesQueryTests(ToursTestFactory factory) : base(factory)
        {
        }

        [Fact]
        public void Retrieves_all()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            path = path.Substring(0, path.IndexOf("bin"));
            var scriptPath = path + "\\TestData\\a-delete-tourPreferences.sql";
            //C:\Users\Nikola\Documents\PSW\Backend\psw-be-ra-2024-group-8\src\Modules\Tours\Explorer.Tours.Tests\TestData\b-insert-tourPreferences.sql
            //scriptPath = Path.GetFullPath(scriptPath);
            var sqlScript = File.ReadAllText(scriptPath);
            //var sqlStatements = sqlScript.Split(';', StringSplitOptions.RemoveEmptyEntries);
            //foreach (var statement in sqlStatements)
            //{     
            if (!string.IsNullOrWhiteSpace(sqlScript))
                {
                    dbContext.Database.ExecuteSqlRaw(sqlScript);
                }
            //}
            // Act
            var newEntity = new TourPreferencesDto
            {
                Difficulty = API.Dtos.DifficultyLevel.EASY,
                WalkRating = 3,
                BikeRating = 2,
                CarRating = 0,
                BoatRating = 1,
                InterestTags = new List<string>() { "nature" }

            };

            // Act
            var createResult = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TourPreferencesDto;

            var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as PagedResult<TourPreferencesDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(1);
            result.TotalCount.ShouldBe(1);
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
