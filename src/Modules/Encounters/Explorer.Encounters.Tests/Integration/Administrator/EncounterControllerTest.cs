using Explorer.API.Controllers.Administrator.Administration;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.Core.Domain.Blogs;
using Explorer.Encounters.API.Controllers;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public.Administrator;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Infrastructure.Database;
using Explorer.Encounters.Tests;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Infrastructure.Database;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shouldly;
using System.Collections.Generic;
using Xunit;
using static Explorer.Encounters.API.Dtos.EncounterDTO;

namespace Explorer.Encounters.Tests.Integration;

[Collection("Sequential Test Collection")]
public class EncounterControllerTest : BaseEncountersIntegrationTest
{
    public EncounterControllerTest(EncountersTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void A_Creates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
        var mapLocationDto = new MapLocationDTO
        {
            Latitude = 45.1234,
            Longitude = 19.5678
        };
        var newEncounter = new EncounterDTO
        {
            Id = 5,
            Name = "Test Encounter",
            Description = "Test Description",
            Location = mapLocationDto,
            XP = 100,
            Status = "DRAFT",
            Type = "SOCIAL",
            PublishedDate = DateTime.UtcNow,
            ArchivedDate = DateTime.UtcNow.AddDays(5),
            AuthorId = 5,
            IsReviewed = false,
            IsRequired = false,
            RequiredParticipants = 8,
            Radius = 10
};

        // Act
        var result = ((ObjectResult)controller.Create(newEncounter).Result)?.Value as EncounterDTO;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Name.ShouldBe(newEncounter.Name);

        // Assert - Database
        var storedEncounter = dbContext.Encounters.FirstOrDefault(e => e.Name == newEncounter.Name);
        storedEncounter.ShouldNotBeNull();
        storedEncounter.Id.ShouldBe(result.Id);
        storedEncounter.XP.ShouldBe(100);
        storedEncounter.Status.ShouldBe(EncounterStatus.DRAFT);
    }

    [Fact]
    public void A_Create_fails_invalid_data()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var invalidEncounter = new EncounterDTO
        {
            Description = "Invalid Test Description",
            XP = -10
        };

        // Act
        var result = (ObjectResult)controller.Create(invalidEncounter).Result;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(400);
    }

    [Fact]
    public void Q_Updates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();

        // Priprema za MapLocationDTO i EncounterDTO
        var mapLocationDto = new MapLocationDTO
        {
            Latitude = 45.1234,
            Longitude = 19.5678
        };

        var updatedEncounter = new EncounterDTO
        {
            Id = 15, // Id je postavljen na validnu vrednost
            Name = "Updated Encounter",
            Description = "Updated Description",
            Location = mapLocationDto,
            XP = 200,
            Status = "ACTIVE",
            Type = "LOCATION",
            AuthorId = 1,
            IsReviewed = true
        };

        // Pretpostavljamo da već postoji Encounter sa Id == 1 u bazi koji će biti ažuriran
        var existingEncounter = dbContext.Encounters.FirstOrDefault(e => e.Id == updatedEncounter.Id);
        existingEncounter.ShouldNotBeNull(); // Provera da entitet postoji pre nego što ga ažuriramo

        // Act - Pozivanje Update metode sa Id i ažuriranim EncounterDTO
        var result = controller.Update(updatedEncounter.Id, updatedEncounter).Result as OkObjectResult;

        // Assert - Response
        result.ShouldNotBeNull();
        var updatedResult = result.Value as EncounterDTO; // Pretpostavljamo da rezultat vraća EncounterDTO
        updatedResult.ShouldNotBeNull();
        updatedResult.Id.ShouldBe(updatedEncounter.Id);
        updatedResult.Name.ShouldBe(updatedEncounter.Name);
        updatedResult.Description.ShouldBe(updatedEncounter.Description);
        updatedResult.XP.ShouldBe(updatedEncounter.XP);

        // Assert - Database - Proveravamo da li su podaci u bazi ažurirani
        var storedEncounter = dbContext.Encounters.FirstOrDefault(e => e.Id == updatedEncounter.Id);
        storedEncounter.ShouldNotBeNull();
        storedEncounter.Name.ShouldBe(updatedEncounter.Name);
        storedEncounter.Description.ShouldBe(updatedEncounter.Description);
        storedEncounter.XP.ShouldBe(updatedEncounter.XP);
    }


    [Fact]
    public void C_Update_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var invalidEncounter = new EncounterDTO
        {
            Id = -1000,
            Name = "Invalid Encounter"
        };

        // Act
        var result = (ObjectResult)controller.Update(invalidEncounter.Id,invalidEncounter).Result;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    [Fact]
    public void Q_ArchiveEncounter_Works()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();

        // Fetch an existing encounter from the database
        var encounter = dbContext.Encounters.FirstOrDefault(e => e.Status == EncounterStatus.ACTIVE);
        encounter.ShouldNotBeNull("An active encounter must exist for this test.");

        // Act
        var result = controller.ArchiveEncounter(encounter.Id) as OkObjectResult;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        var archivedEncounter = dbContext.Encounters.FirstOrDefault(e => e.Id == encounter.Id);
        archivedEncounter.ShouldNotBeNull();
        archivedEncounter.Status.ShouldBe(EncounterStatus.ARCHIVED); // Assuming ARCHIVED is a valid status
    }

    [Fact]
    public void X_PublishEncounter_Works()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();

        // Direktno izvršavamo SQL upit za unos podataka
        string sqlScript = @"
        INSERT INTO encounters.encounters (""Id"", ""Name"", ""Description"", ""XP"", ""Status"", ""Type"", ""AuthorId"", ""Location_Latitude"", ""Location_Longitude"", ""IsReviewed"", ""PublishedDate"", ""ArchivedDate"")
        VALUES 
        (23, 'Old Encounter', 'Old Description', 50, 'ACTIVE', 'SOCIAL', 1, 45.1234, 19.5678, false, NOW(), NOW()),
        (25, 'Old Encounter', 'Old Description', 50, 'DRAFT', 'SOCIAL', 1, 45.1234, 19.5678, false, NOW(), NOW()),
        (28, 'Old Encounter', 'Old Description', 50, 'ACTIVE', 'SOCIAL', 1, 45.1234, 19.5678, false, NOW(), NOW());";

        // Izvršavamo SQL upit
        dbContext.Database.ExecuteSqlRaw(sqlScript);

        // Fetch an existing encounter from the database
        var encounter = dbContext.Encounters.FirstOrDefault(e => e.Status == EncounterStatus.DRAFT);
        encounter.ShouldNotBeNull("A draft encounter must exist for this test.");

        // Act
        var result = controller.PublishEncounter(encounter.Id) as OkObjectResult;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        var publishedEncounter = dbContext.Encounters.FirstOrDefault(e => e.Id == encounter.Id);
        publishedEncounter.ShouldNotBeNull();
        publishedEncounter.Status.ShouldBe(EncounterStatus.ACTIVE); // Assuming ACTIVE is the result of publishing
       

    }


    [Fact]
    public void Q_MarkAsReviewed_Works()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();

        // Fetch an existing encounter from the database
        var encounter = dbContext.Encounters.FirstOrDefault(e => e.IsReviewed == false);
        encounter.ShouldNotBeNull("An unreviewed encounter must exist for this test.");

        // Act
        var result = controller.MarkAsReviewed(encounter.Id) as OkObjectResult;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        var reviewedEncounter = dbContext.Encounters.FirstOrDefault(e => e.Id == encounter.Id);
        reviewedEncounter.ShouldNotBeNull();
        reviewedEncounter.IsReviewed.ShouldBeTrue();
    }

    [Fact]
    public void ArchiveEncounter_Fails_InvalidId()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = (ObjectResult)controller.ArchiveEncounter(-1000);

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    [Fact]
    public void PublishEncounter_Fails_InvalidId()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = (ObjectResult)controller.PublishEncounter(-1000);

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    [Fact]
    public void MarkAsReviewed_Fails_InvalidId()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = (ObjectResult)controller.MarkAsReviewed(-1000);

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    [Fact]
    public void X_Deletes()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
        var encounter = dbContext.Encounters.FirstOrDefault();
        var result = controller.Delete(encounter.Id) as OkObjectResult;
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200); // Proverava status kod
        result.Value.ShouldNotBeNull();  // Proverava da je vraćen sadržaj

        // Assert - Proveravamo da je entitet obrisan iz baze
        var storedEncounter = dbContext.Encounters.FirstOrDefault(e => e.Id == encounter.Id);
        storedEncounter.ShouldBeNull();
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

    private static EncounterController CreateController(IServiceScope scope)
    {
        return new EncounterController(scope.ServiceProvider.GetRequiredService<IEncounterService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}