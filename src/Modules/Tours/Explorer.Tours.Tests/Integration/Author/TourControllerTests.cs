using Explorer.API.Controllers.Author;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Author;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Explorer.Tours.API.Public.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Tests.Integration.Author
{
    [Collection("Sequential")]
    public class TourControllerTests : BaseToursIntegrationTest
    {
        public TourControllerTests(ToursTestFactory factory) : base(factory)
        {
        }

        [Fact]
        public void Creates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newTour = new TourDTO
            {
                Name = "Nova Tura",
                Description = "Opis nove ture.",
                Weight = "5kg",
                Tags = new[] { "avantura", "priroda" },
                Price = 100.00m
            };

            // Act
            var result = ((ObjectResult)controller.Create(newTour).Result)?.Value as TourDTO;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.Name.ShouldBe(newTour.Name);

            // Assert - Database
            var storedTour = dbContext.Tours.FirstOrDefault(i => i.Name == newTour.Name);
            storedTour.ShouldNotBeNull();
            storedTour.Id.ShouldBe(result.Id);
        }

        [Fact]
        public void Create_fails_invalid_data()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var invalidTour = new TourDTO
            {
                Description = "Opis bez imena" // Nedostaje Name
            };

            // Act
            var result = (ObjectResult)controller.Create(invalidTour).Result;

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
            var updatedTour = new TourDTO
            {
                Id = 1, // Pretpostavljamo da entitet sa ID 1 postoji
                Name = "Izmenjena Tura",
                Description = "Izmenjen opis nove ture.",
                Weight = "6kg",
                Tags = new[] { "avantura", "priroda", "izmenjeno" },
                Price = 150.00m
            };

            var result = ((ObjectResult)controller.Update(updatedTour).Result)?.Value as TourDTO;

            result.ShouldNotBeNull();
            result.Id.ShouldBe(updatedTour.Id);
            result.Name.ShouldBe(updatedTour.Name);
            result.Description.ShouldBe(updatedTour.Description);

            var storedTour = dbContext.Tours.FirstOrDefault(i => i.Id == updatedTour.Id);
            storedTour.ShouldNotBeNull();
            storedTour.Description.ShouldBe(updatedTour.Description);
        }

        [Fact]
        public void Update_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var invalidTour = new TourDTO
            {
                Id = -1000,
                Name = "Test"
            };

            var result = (ObjectResult)controller.Update(invalidTour).Result;

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

            var storedTour = dbContext.Tours.FirstOrDefault(i => i.Id == 1);
            storedTour.ShouldBeNull();
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

        private static TourController CreateController(IServiceScope scope)
        {
            return new TourController(scope.ServiceProvider.GetRequiredService<ITourService>())
            {
                ControllerContext = BuildContext("-1") 
            };
        }
    }
}
