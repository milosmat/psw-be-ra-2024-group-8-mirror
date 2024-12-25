using Explorer.API.Controllers.Administrator.Administration;
using Explorer.API.Controllers.Author;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.API.Public.Author;
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

namespace Explorer.Tours.Tests.Integration.Administration
{
    [Collection("Sequential")]
    public class TourCheckpointCommandTests : BaseToursIntegrationTest
    {
        public TourCheckpointCommandTests(ToursTestFactory factory) : base(factory) { }


        [Fact]
        public void Retrieves_all()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = ((ObjectResult)controller.GetAll(1, 100).Result)?.Value as PagedResult<TourCheckpointDto>;

            // Assert
            result.ShouldNotBeNull();
            //result.Results.Count.ShouldBe(3);
            //result.TotalCount.ShouldBe(3);
        }

        /*[Fact]
        public void Create()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            //var basePath = AppDomain.CurrentDomain.BaseDirectory;
            //var scriptPath = Path.Combine(basePath, "TestData", "a-insert-tours.sql");
            var scriptPath = "C:\\Users\\Nikola\\Documents\\PSW\\Backend\\psw-be-ra-2024-group-8\\src\\Modules\\Tours\\Explorer.Tours.Tests\\TestData\\a-insert-tours.sql";
            var sqlScript = File.ReadAllText(scriptPath); // Ensure the file path is correct
            dbContext.Database.ExecuteSqlRaw(sqlScript);
            var newEntity = new TourCheckpointDto
            {
                Latitude = 42.258,
                Longitude = 19.258,
                CheckpointName = "Test",
                CheckpointDescription = "Opis",
                TourId = 1,
                Image = "Image"
            };
            // Act
            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TourCheckpointDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.CheckpointName.ShouldBe(newEntity.CheckpointName);

            // Assert - Database
            var storedEntity = dbContext.TourCheckpoints.FirstOrDefault(i => i.CheckpointName == newEntity.CheckpointName);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldBe(result.Id);
        }*/
        private static TourCheckpointController CreateController(IServiceScope scope)
        {
            return new TourCheckpointController(scope.ServiceProvider.GetRequiredService<ITourCheckpointService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
