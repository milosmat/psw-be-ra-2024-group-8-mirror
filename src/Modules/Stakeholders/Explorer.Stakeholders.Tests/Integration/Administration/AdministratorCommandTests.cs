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
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Tests.Integration.Administration
{
    [Collection("Sequential")]
    public class AdministratorCommandTests : BaseStakeholdersIntegrationTest
    {
        public AdministratorCommandTests(StakeholdersTestFactory factory) : base(factory) { }
       

        [Theory]
        [InlineData(1, "username1", "password123", UserRole.Tourist, true)]
        [InlineData(1, "username2", "password123", UserRole.Tourist, false)]
        public void Blocks_Unblock_User_Account_Succeeds(int userId, string username, string password, UserRole role, bool isActive)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var controller = CreateController(scope);

            bool updatedAccountStatus = !isActive;

            var testAccount = new AccountInformationDto
            {
                Id = userId,
                Username = username,
                Role = role.ToString(),
                IsActive = updatedAccountStatus
            };

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            string sqlScript = @"
                        DELETE FROM stakeholders.""Users"";
                        INSERT INTO stakeholders.""Users""(
	                        ""Id"", ""Username"", ""PasswordHash"", ""Role"", ""IsActive"")
	                        VALUES({0}, {1}, {2}, {3}, {4});
                        INSERT INTO stakeholders.""People""(
	                        ""Id"", ""UserId"", ""Name"", ""Surname"", ""Email"")
	                        VALUES ({5}, {6}, {7}, {8}, {9});
                         ";

            dbContext.Database.ExecuteSqlRaw(sqlScript, userId, username, passwordHash, ((int)role), isActive,
                                                        1, userId, "testName", "testSurname", "test@gmail.com");


            // Act
            var result = (ObjectResult)controller.Update(testAccount).Result;

            // Assert - Response
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            
            // Assert - Database
            var storedEntity = dbContext.Users.FirstOrDefault(i => i.Username == testAccount.Username);
            storedEntity.ShouldNotBeNull();
            storedEntity.IsActive.ShouldBe(testAccount.IsActive);

        }

      /*  [Fact]
        public void Update_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new AccountInformationDto
            {
                Id = -1000,
                Username = "admin@gmail.com",
               // Password = "admin",
                Role = "0",
                IsActive = false
            };

            // Act
            var result = (ObjectResult)controller.Update(updatedEntity).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }
      */
        private static AccountController CreateController(IServiceScope scope)
        {
            return new AccountController(scope.ServiceProvider.GetRequiredService<IAdministratorService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
