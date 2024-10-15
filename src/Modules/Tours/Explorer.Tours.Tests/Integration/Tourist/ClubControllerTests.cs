using Explorer.API.Controllers.Author;
using Explorer.API.Controllers.Tourist;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Author;
using Explorer.Tours.API.Public.Tourist;
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
    [Collection("Sequential")]
    public class ClubControllerTests : BaseToursIntegrationTest
    {
        public ClubControllerTests(ToursTestFactory factory) : base(factory)
        {
        }

        private static ClubController CreateController(IServiceScope scope)
        {
            return new ClubController(scope.ServiceProvider.GetRequiredService<IClubService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }

        [Fact]
        public void Creates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newClub = new ClubDto
            {
                Name = "Planinari avanturisti",
                Description = "Klub za ljubitelje planinarenja i avantura",
                Photo = "planinari.jpg",
                OwnerId = 1
            };

            // Act
            var result = ((ObjectResult)controller.Create(newClub).Result)?.Value as ClubDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.Name.ShouldBe(newClub.Name);

            // Assert - Database
            var storedClub = dbContext.Clubs.FirstOrDefault(i => i.Name == newClub.Name);
            storedClub.ShouldNotBeNull();
            storedClub.Id.ShouldBe(result.Id);
        }

        [Fact]
        public void Create_fails_invalid_data()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var invalidClub = new ClubDto
            {
                Description = "Opis nevazeceg kluba" // Nedostaju Name, Photo, OwnerId
            };

            // Act
            var result = (ObjectResult)controller.Create(invalidClub).Result;

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
            var updatedClub = new ClubDto
            {
                Id = 1, // Pretpostavljamo da entitet sa ID 1 postoji
                Name = "Planinari avanturisti - izmjena",
                Description = "Klub za ljubitelje planinarenja i avantura",
                Photo = "planinari.jpg",
                OwnerId = 1
            };

            var result = ((ObjectResult)controller.Update(updatedClub).Result)?.Value as ClubDto;

            result.ShouldNotBeNull();
            result.Id.ShouldBe(updatedClub.Id);
            result.Name.ShouldBe(updatedClub.Name);
            result.Description.ShouldBe(updatedClub.Description);
            result.Photo.ShouldBe(updatedClub.Photo);
            result.OwnerId.ShouldBe(updatedClub.OwnerId);


            var storedClub = dbContext.Clubs.FirstOrDefault(i => i.Id == updatedClub.Id);
            storedClub.ShouldNotBeNull();
            storedClub.Description.ShouldBe(updatedClub.Description);
            storedClub.Name.ShouldBe(updatedClub.Name);
            storedClub.Photo.ShouldBe(updatedClub.Photo);
        }

        [Fact]
        public void Update_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var invalidClub = new ClubDto
            {
                Id = -1000,
                Name = "Test"
            };

            var result = (ObjectResult)controller.Update(invalidClub).Result;

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

            var storedClub = dbContext.Clubs.FirstOrDefault(i => i.Id == 1);
            storedClub.ShouldBeNull();
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



    }
}
