using Explorer.API.Controllers.Administrator.Administration;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
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
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.API.Public.Administration;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Tests.Integration.Administration
{
    [Collection("Sequential")]
    public class AdministratorQueryTests : BaseStakeholdersIntegrationTest
    {
        public AdministratorQueryTests(StakeholdersTestFactory factory) : base(factory) { }

        [Theory]
        [InlineData(-1, "usernameA", "password123", UserRole.Tourist, true)]
        [InlineData(-2, "usernameB", "password123", UserRole.Tourist, false)]
        [InlineData(-3, "usernameC", "password123", UserRole.Tourist, false)]
        public void Retrieves_all(int userId, string username, string password, UserRole role, bool isActive)
        {
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var controller = CreateController(scope);

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            string sqlScript = @"
                        INSERT INTO stakeholders.""Users""(
	                        ""Id"", ""Username"", ""PasswordHash"", ""Role"", ""IsActive"")
	                        VALUES({0}, {1}, {2}, {3}, {4});
                        INSERT INTO stakeholders.""People""(
	                        ""Id"", ""UserId"", ""Name"", ""Surname"", ""Email"")
	                        VALUES ({5}, {6}, {7}, {8}, {9});
                         ";

            dbContext.Database.ExecuteSqlRaw(sqlScript, userId, username, passwordHash, ((int)role), isActive,
                                                        userId, userId, "name", "surname", "test@gmail.com");


            var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as PagedResult<AccountInformationDto>;

            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(dbContext.Users.Count());
            result.TotalCount.ShouldBe(dbContext.Users.Count());
        
        }

        private static AccountController CreateController(IServiceScope scope)
        {
            return new AccountController(scope.ServiceProvider.GetRequiredService<IAdministratorService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
