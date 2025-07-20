using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Explorer.API.Controllers;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Payments.API.Public.Tourist;
using FluentResults;
using Moq;
using Microsoft.EntityFrameworkCore;
using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Stakeholders.Tests.Integration.Authentication;

[Collection("Sequential")]
public class RegistrationTests : BaseStakeholdersIntegrationTest
{
    public RegistrationTests(StakeholdersTestFactory factory) : base(factory) { }

    [Fact]
    public async Task Registration_ValidData_Succeeds()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        
        var emailServiceMock = new Mock<IEmailService>();
        emailServiceMock.Setup(s => s.SendEmailToUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(Result.Ok()); //simuliraj izvrsavanje servisa da uvijek bude uspjesno

        var controller = CreateController(scope, emailServiceMock.Object);

        var account = new AccountRegistrationDto
        {
            Username = "turistaA@gmail.com",
            Email = "turistaA@gmail.com",
            Password = "turistaA",
            Name = "Žika",
            Surname = "Žikić"
        };

        // Act
        var result = await controller.Register(account);

        // Assert - Response
        Assert.IsType<OkObjectResult>(result);  
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);

        // Assert - Database
        dbContext.ChangeTracker.Clear();
        var storedAccount = dbContext.Users.FirstOrDefault(u => u.Username == account.Email);
        storedAccount.ShouldNotBeNull();
        storedAccount.Role.ShouldBe(UserRole.Tourist);
        storedAccount.PasswordHash.ShouldNotBeNull();
        BCrypt.Net.BCrypt.Verify(account.Password, storedAccount.PasswordHash).ShouldBeTrue();

        var storedPerson = dbContext.People.FirstOrDefault(i => i.Email == account.Email);
        storedPerson.ShouldNotBeNull();
        storedPerson.UserId.ShouldBe(storedAccount.Id);
        storedPerson.Name.ShouldBe(account.Name); 
        
        // Provjeri da li je metoda pozvana prilikom registracije
        emailServiceMock.Verify(service => service.SendEmailToUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Registration_DuplicateUsername_Fails()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        
        var emailServiceMock = new Mock<IEmailService>();
        emailServiceMock.Setup(s => s.SendEmailToUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(Result.Ok()); //simuliraj izvrsavanje servisa da uvijek bude uspjesno

        var controller = CreateController(scope, emailServiceMock.Object);


        var account1 = new AccountRegistrationDto
        {
            Username = "turistaB",
            Email = "turistaB@gmail.com",
            Password = "turistaB",
            Name = "Pera",
            Surname = "Perić"
        };

        await controller.Register(account1);

        var account = new AccountRegistrationDto
        {
            Username = "turistaB",
            Email = "mika@gmail.com",
            Password = "mika123",
            Name = "Mika",
            Surname = "Mikić"
        };

        // Act
        var result = await controller.Register(account);

        // Assert - Response
        var failedResult = result as ObjectResult;
        Assert.NotNull(failedResult);
        Assert.Equal(409, failedResult.StatusCode);

        // Assert - Database
        dbContext.ChangeTracker.Clear();
        var storedAccount = dbContext.Users.FirstOrDefault(u => u.Username == account.Email);
        storedAccount.ShouldBeNull();
        
        var storedPerson = dbContext.People.FirstOrDefault(i => i.Email == account.Email);
        storedPerson.ShouldBeNull();
        
        // Execute SQL script
        string sqlScript = @"
                DELETE FROM stakeholders.""Users"" WHERE ""Username"" = 'turistaB';"; 
        dbContext.Database.ExecuteSqlRaw(sqlScript);
        
    }

    private static AuthenticationController CreateController(IServiceScope scope, IEmailService? emailServiceMock)
    {
        return new AuthenticationController(scope.ServiceProvider.GetRequiredService<IAuthenticationService>(),
            emailServiceMock ?? scope.ServiceProvider.GetRequiredService<IEmailService>(),
            scope.ServiceProvider.GetRequiredService<IWalletService>());
    }
}