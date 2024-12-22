using Explorer.API.Controllers.Administrator.Administration;
using Explorer.Encounters.API.Controllers;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public.Administrator;
using Explorer.Encounters.Infrastructure.Database;
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
using Xunit;

namespace Explorer.Encounters.Tests.Integration.Administrator
{
    [Collection("Sequential")]
    public class EncounterCommandTests : BaseEncountersIntegrationTest
    {
        public EncounterCommandTests(EncountersTestFactory factory) : base(factory) { }

        [Fact]
        public void Creates()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
            var newEntity = new EncounterDTO
            {
                Name = "izazov1",
                Description = "Opis1",
                Location = new EncounterDTO.MapLocationDTO
                {
                    Latitude = 45.258,
                    Longitude = 19.254
                },
                XP = 50,
                Status = "ACTIVE",
                Type = "MISC",
                PublishedDate = DateTime.Now,
                ArchivedDate = null,
                AuthorId = 1,
                Image = null,
                UsersWhoCompletedId = null,
                IsReviewed = false,
                IsRequired = false

            };
            // Act
            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as EncounterDTO;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.Name.ShouldBe(newEntity.Name);

            // Assert - Database
            var storedEntity = dbContext.Encounters.FirstOrDefault(i => i.Name == newEntity.Name);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldBe(result.Id);
        }
        [Fact]
        public void UserCompletesEncounter()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
            var newEntity = new EncounterDTO
            {
                Name = "izazov1",
                Description = "Opis1",
                Location = new EncounterDTO.MapLocationDTO
                {
                    Latitude = 45.258,
                    Longitude = 19.254
                },
                XP = 50,
                Status = "ACTIVE",
                Type = "MISC",
                PublishedDate = DateTime.Now,
                ArchivedDate = null,
                AuthorId = 1,
                Image = null,
                UsersWhoCompletedId = new List<long>(),
                IsReviewed = false,
                IsRequired = false

            };
            // Act
            
            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as EncounterDTO;
            result.UsersWhoCompletedId.Add(2);
            var updateResult = ((ObjectResult)controller.Update(Convert.ToInt32(result.Id),result).Result)?.Value as EncounterDTO;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.Name.ShouldBe(newEntity.Name);
            //result.UsersWhoCompletedId.ShouldContain(2);
            updateResult.ShouldNotBeNull();
            //updateResult.UsersWhoCompletedId.ShouldContain(2);

            // Assert - Database
            var storedEntity = dbContext.Encounters.FirstOrDefault(i => i.Id == result.Id);
            storedEntity.ShouldNotBeNull();
            storedEntity.Name.ShouldBe(result.Name);
        }
    private static EncounterController CreateController(IServiceScope scope)
    {
        return new EncounterController(scope.ServiceProvider.GetRequiredService<IEncounterService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }

    }
    
}
