using Explorer.API.Controllers.Tourist;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Infrastructure.Database;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Tests.Integration.Administration
{
    public class ClubCommandTests : BaseStakeholdersIntegrationTest
    {
        public ClubCommandTests(StakeholdersTestFactory factory) : base(factory) { }

        [Theory]
        [InlineData("Club A", "Description A", "photoA.png", -11)]
        [InlineData("Club B", "Description B", "photoB.png", -12)]
        public void CreatesClub(string name, string description, string photo, int ownerId)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope); // Metoda koja kreira instancu kontrolera
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            var newClub = new ClubDto
            {
                Name = name,
                Description = description,
                Photo = photo,
                OwnerId = ownerId,
                MembershipRequest = new List<MembershipRequestDto>() // Initialize as empty
            };

            // Act
            var result = ((ObjectResult)controller.Create(newClub).Result)?.Value as ClubDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Name.ShouldBe(newClub.Name);
            result.Description.ShouldBe(newClub.Description);
            result.Photo.ShouldBe(newClub.Photo);
            result.OwnerId.ShouldBe(newClub.OwnerId);

            // Assert - Database
            var storedClub = dbContext.Clubs.FirstOrDefault(c => c.Name == newClub.Name);
            storedClub.ShouldNotBeNull();
            storedClub.Name.ShouldBe(newClub.Name);
            storedClub.Description.ShouldBe(newClub.Description);
            storedClub.Photo.ShouldBe(newClub.Photo);
            storedClub.OwnerId.ShouldBe(newClub.OwnerId);
        }

        [Theory]
        [InlineData(null, "TestDescription", 500)]
        public void Create_fails_invalid_data(string name, string description, int expectedStatusCode)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var newEntity = new ClubDto
            {
                Name = name,
                Description = description,
                MembershipRequest = new List<MembershipRequestDto>()
            };

            // Act
            var result = (ObjectResult)controller.Create(newEntity).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(expectedStatusCode);
        }


        [Theory]
        [InlineData(-2, "UpdateName2", "UpdateDescription2", "UpdatePhoto2")]
        public void Updates(int id, string name, string description, string photo)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            var updatedEntity = new ClubDto
            {
                Id = id,
                Name = name,
                Description = description,
                Photo = photo
            };

            // Act
            var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as ClubDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(id);
            result.Name.ShouldBe(updatedEntity.Name);
            result.Description.ShouldBe(updatedEntity.Description);

            // Assert - Database
            var storedEntity = dbContext.Clubs.FirstOrDefault(i => i.Id == id);
            storedEntity.ShouldNotBeNull();
            storedEntity.Name.ShouldBe(name);
            storedEntity.Description.ShouldBe(description);
        }

        [Theory]
        [InlineData(-1000, "Test", "Description", "Photo", 500)]
        public void Update_fails_invalid_id(int id, string name, string description, string photo, int expectedStatusCode)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new ClubDto
            {
                Id = id,
                Name = name,
                Description = description,
                Photo = photo
            };

            // Act
            var result = (ObjectResult)controller.Update(updatedEntity).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(expectedStatusCode);
        }

        [Theory]
        [InlineData("Create", "Description", "Image.png" , true)] // Expected to exist and be deleted
        public void Deletes(string name, string description, string photo, bool shouldExist)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            var newClub = new ClubDto
            {
                Name = "Initial title",
                Description = "Initial description",
                Photo = photo,
                MembershipRequest = new List<MembershipRequestDto>() // Initialize as empty
            };
            var createdClub = ((ObjectResult)controller.Create(newClub).Result)?.Value as BlogsDto;
            var createdClubId = createdClub?.Id ?? -1;

            // Pre-check: Verify the initial existence state based on shouldExist
            var preDeleteEntity = dbContext.Clubs.FirstOrDefault(b => b.Id == createdClubId);
            if (shouldExist)
            {
                preDeleteEntity.ShouldNotBeNull();
            }
            else
            {
                preDeleteEntity.ShouldBeNull();
            }

            // Act: Call Delete method
            controller.Delete(createdClubId);

            // Assert - Check if entity was deleted from the database
            var postDeleteEntity = dbContext.Clubs.FirstOrDefault(i => i.Id == createdClubId);
            postDeleteEntity.ShouldBeNull(); // Should be null if successfully deleted or if it didn't exist initially
        }

        [Theory]
        [InlineData(-1000, 404)]
        public void Delete_fails_invalid_id(int id, int expectedStatusCode)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = (ObjectResult)controller.Delete(id);

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(expectedStatusCode);
        }


        private static ClubController CreateController(IServiceScope scope)
        {
            return new ClubController(scope.ServiceProvider.GetRequiredService<IClubService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
