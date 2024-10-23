using Explorer.Tours.API.Dtos;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Explorer.API.Controllers.Administrator.Administration;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Public.Administration;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Stakeholders.API.Dtos;

namespace Explorer.Stakeholders.Tests.Integration.Administration
{
    [Collection("Sequential")]
    public class AdministratorCommandTests : BaseStakeholdersIntegrationTest
    {
        public AdministratorCommandTests(StakeholdersTestFactory factory) : base(factory) { }
        private static AccountController CreateController(IServiceScope scope)
        {
            return new AccountController(scope.ServiceProvider.GetRequiredService<IAdministratorService>(),
                                         scope.ServiceProvider.GetRequiredService<ICrudRepository<Person>>())
            {
                ControllerContext = BuildContext("-1")
            };
        }

        [Fact]
        public void Updates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var updatedEntity = new AccountInformationDto
            {
                Id = -1,
                Username = "admin@gmail.com",
                Password = "admin",
                Role = "0",
                IsActive = false
            };

            // Act
            var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as AccountInformationDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(-1);
            result.Username.ShouldBe(updatedEntity.Username);
            result.IsActive.ShouldBe(updatedEntity.IsActive);

            // Assert - Database
            var storedEntity = dbContext.Users.FirstOrDefault(i => i.Username == "admin@gmail.com");
            storedEntity.ShouldNotBeNull();
            storedEntity.IsActive.ShouldBe(updatedEntity.IsActive);

        }

        [Fact]
        public void Update_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new AccountInformationDto
            {
                Id = -1000,
                Username = "admin@gmail.com",
                Password = "admin",
                Role = "0",
                IsActive = false
            };

            // Act
            var result = (ObjectResult)controller.Update(updatedEntity).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }
    }
}
