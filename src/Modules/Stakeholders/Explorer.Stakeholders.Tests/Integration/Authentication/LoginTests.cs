using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Explorer.API.Controllers;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Payments.API.Public.Tourist;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using FluentResults;
using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Stakeholders.Tests.Integration.Authentication;

[Collection("Sequential")]
public class LoginTests : BaseStakeholdersIntegrationTest
{
    public LoginTests(StakeholdersTestFactory factory) : base(factory) { }

    [Fact]
    public void Login_ValidCredentials_Succeeds()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        var controller = CreateController(scope);

        var loginSubmission = new CredentialsDto 
        {
            Username = "turista1", 
            Password = "turista1"
        };

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(loginSubmission.Password);

        string sqlScript = @"
                        DELETE FROM stakeholders.""Users"";
                        INSERT INTO stakeholders.""Users""(
	                        ""Id"", ""Username"", ""PasswordHash"", ""Role"", ""IsActive"")
	                        VALUES({0}, {1}, {2}, {3}, {4});
                        INSERT INTO stakeholders.""People""(
	                        ""Id"", ""UserId"", ""Name"", ""Surname"", ""Email"")
	                        VALUES ({5}, {6}, {7}, {8}, {9});
                         ";

        dbContext.Database.ExecuteSqlRaw(sqlScript, 1, loginSubmission.Username, passwordHash, 2, true,
                                                    1, 1, "Pavle", "Pavlovic", "pavle@gmail.com");
        //Act
        var result = (ObjectResult)controller.Login(loginSubmission).Result;

        // Assert - Response
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);

        var authenticationResponse = result.Value as AuthenticationTokensDto;
        authenticationResponse.ShouldNotBeNull();

        // Decode JWT token
        var decodedAccessToken = new JwtSecurityTokenHandler().ReadJwtToken(authenticationResponse.AccessToken);
        var personId = decodedAccessToken.Claims.FirstOrDefault(c => c.Type == "personId");
        personId.ShouldNotBeNull();
    }

    [Fact]
    public void Login_WhenUserNotRegistered_Fails()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var loginSubmission = new CredentialsDto
        {
            Username = "turistaY", 
            Password = "turista1"
        };

        // Act
        var result = (ObjectResult)controller.Login(loginSubmission).Result;

        // Assert
        result.StatusCode.ShouldBe(404);
    }

    [Fact]
    public void Login_InvalidPassword_Fails()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        var controller = CreateController(scope);

        var loginSubmission = new CredentialsDto
        {
            Username = "turistaZ",
            Password = "turistaZ"
        };

        string storedPassword = "password123";
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(storedPassword);

        string sqlScript = @"
                        DELETE FROM stakeholders.""Users"";
                        INSERT INTO stakeholders.""Users""(
	                        ""Id"", ""Username"", ""PasswordHash"", ""Role"", ""IsActive"")
	                        VALUES({0}, {1}, {2}, {3}, {4});
                        INSERT INTO stakeholders.""People""(
	                        ""Id"", ""UserId"", ""Name"", ""Surname"", ""Email"")
	                        VALUES ({5}, {6}, {7}, {8}, {9});
                         ";

        dbContext.Database.ExecuteSqlRaw(sqlScript, 1, loginSubmission.Username, passwordHash, 2, true,
                                                    1, 1, "Marko", "Markovic", "marko@gmail.com");

        // Act
        var result = (ObjectResult)controller.Login(loginSubmission).Result;

        // Assert
        result.StatusCode.ShouldBe(400);
    }

    private static AuthenticationController CreateController(IServiceScope scope)
    {
        return new AuthenticationController(scope.ServiceProvider.GetRequiredService<IAuthenticationService>(),
            scope.ServiceProvider.GetRequiredService<IEmailService>(),
            scope.ServiceProvider.GetRequiredService<IWalletService>());
    }
}