using Explorer.API.Controllers.Tourist;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public.Tourist;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Infrastructure.Database;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Collections.Generic;
using Xunit;

namespace Explorer.Encounters.Tests.Integration.Tourist
{
    [Collection("Sequential")]
    public class TouristControllerTest : BaseEncountersIntegrationTest
    {
        public TouristControllerTest(EncountersTestFactory factory) : base(factory)
        {
        }

        [Fact]
        public void GetTouristById_Works()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();

            // Directly execute SQL queries to insert data
            string sqlScript = @"
                DELETE FROM encounters.tourist_profiles;
                INSERT INTO encounters.tourist_profiles
<<<<<<< HEAD
                (""Id"", ""Username"", ""Password"", ""Role"", ""IsActive"", ""XP"", ""CompletedEncountersIds"", ""CouponIds"")
                VALUES
                (1, 'testuser', 'password123', 2, true, 100, '[1, 2]', '{{1, 2}}'); -- testuser je završio encountere 1 i 2
=======
                (""Id"", ""Username"", ""PasswordHash"", ""Role"", ""IsActive"", ""XP"", ""CompletedEncountersIds"", ""CouponIds"")
                VALUES
                (1, 'testuser', 'password123', 2, true, 100, '[1, 2]','{{}}'); -- testuser je završio encountere 1 i 2
>>>>>>> f01912a501661465cd965870a13f0d766e55e9c8
            ";
            // Execute SQL script
            dbContext.Database.ExecuteSqlRaw(sqlScript);
            var touristId = 1;

            // Act
            var result = controller.GetTouristById(touristId);

            // Assert
            result.ShouldNotBeNull();
            result.Result.ShouldBeOfType<OkObjectResult>();

            sqlScript = @"
                DELETE FROM encounters.tourist_profiles;
            ";

            // Execute SQL script
            dbContext.Database.ExecuteSqlRaw(sqlScript);
        }

        [Fact]
        public void GetTouristById_Fails_InvalidId()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = controller.GetTouristById(-1000);

            // Assert
            result.ShouldNotBeNull();
            result.Result.ShouldBeOfType<BadRequestObjectResult>();

            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.StatusCode.ShouldBe(400);
        }

        [Fact]
        public void AddXPToTourist_Works()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
            // Directly execute SQL queries to insert data
            string sqlScript = @"
                DELETE FROM encounters.tourist_profiles;
                INSERT INTO encounters.tourist_profiles
                (""Id"", ""Username"", ""PasswordHash"", ""Role"", ""IsActive"", ""XP"", ""CompletedEncountersIds"", ""CouponIds"")
                VALUES
                (1, 'testuser', 'password123', 2, true, 100, '[1, 2]', '{{}}'); -- testuser je završio encountere 1 i 2
            ";

            // Execute SQL script
            dbContext.Database.ExecuteSqlRaw(sqlScript);

            long touristId = 1;
            var xpToAdd = 50;

            // Act
            var result = controller.AddXPToTourist(touristId, xpToAdd) as OkResult;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);

            var tourist = dbContext.TouristProfiles.Find(1L);
            tourist.ShouldNotBeNull();
            tourist.XP.ShouldBeGreaterThanOrEqualTo(xpToAdd);

            sqlScript = @"
                DELETE FROM encounters.tourist_profiles;
            ";

            // Execute SQL script
            dbContext.Database.ExecuteSqlRaw(sqlScript);
        }

        [Fact]
        public void AddXPToTourist_Fails_NegativeXP()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = controller.AddXPToTourist(1, -10) as BadRequestObjectResult;

            // Assert
            result.ShouldNotBeNull();

            result.Value.ShouldBe("XP must be a positive number.");
        }

        [Fact]
        public void CompleteEncounter_Works()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();

            // Direktno izvršavamo SQL upite za unos podataka
            string sqlScript = @"
                INSERT INTO encounters.encounters (""Id"", ""Name"", ""Description"", ""XP"", ""Status"", ""Type"", ""AuthorId"", ""Location_Latitude"", ""Location_Longitude"", ""IsReviewed"", ""PublishedDate"", ""ArchivedDate"")
                VALUES 
                (15, 'Old Encounter', 'Old Description', 50, 'ACTIVE', 'SOCIAL', 1, 45.1234, 19.5678, false, NOW(), NOW());

                INSERT INTO encounters.tourist_profiles
                (""Id"", ""Username"", ""PasswordHash"", ""Role"", ""IsActive"", ""XP"", ""CompletedEncountersIds"", ""CouponIds"")
                VALUES
                (-1, 'testUser', 'password123', 2, true, 100, '[1, 2]', '{{}}'), -- testuser je završio encountere 1 i 2
                (-2, 'adventurer', 'adventure', 2, true, 150, '[3, 4, 5]', '{{}}'), -- adventurer je završio encountere 3, 4, 5
                (-3, 'explorer', 'exploremore', 2, true, 200, '[6]', '{{}}'); -- explorer je završio encounter 6
            ";

            // Izvršavamo SQL upite
            dbContext.Database.ExecuteSqlRaw(sqlScript);

            // Fetch an existing encounter and tourist from the database
            var encounter = dbContext.Encounters.FirstOrDefault();
            var tourist = dbContext.TouristProfiles.FirstOrDefault();
            var touristId = tourist.Id;
            var encounterId = encounter.Id;

            // Act
            var result = controller.CompleteEncounter(touristId, encounterId) as OkResult;
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);

            sqlScript = @"
                DELETE FROM encounters.tourist_profiles;
            ";

            // Execute SQL script
            dbContext.Database.ExecuteSqlRaw(sqlScript);
        }


       /* [Fact]
        public void SyncCompletedEncounters_Works()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();

            // Directly execute SQL queries to insert data
            string sqlScript = @"
                DELETE FROM encounters.tourist_profiles;
                INSERT INTO encounters.tourist_profiles
                (""Id"", ""Username"", ""PasswordHash"", ""Role"", ""IsActive"", ""XP"", ""CompletedEncountersIds"")
                VALUES
                (11, 'testuser', 'password123', 2, true, 100, '[1, 2]'); -- testuser je završio encountere 1 i 2
            ";

            // Execute SQL script
            dbContext.Database.ExecuteSqlRaw(sqlScript);

            var username = "testuser";

            // Act
            var result = controller.SyncCompletedEncounters(username);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<OkResult>();

            sqlScript = @"
                DELETE FROM encounters.tourist_profiles;
            ";

            // Execute SQL script
            dbContext.Database.ExecuteSqlRaw(sqlScript);
        }
       */
        [Fact]
        public void GetTouristsByLevel_Works()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();

            // Directly execute SQL queries to insert data
            string sqlScript = @"
                DELETE FROM encounters.tourist_profiles;
                INSERT INTO encounters.tourist_profiles
                (""Id"", ""Username"", ""PasswordHash"", ""Role"", ""IsActive"", ""XP"", ""CompletedEncountersIds"", ""CouponIds"")
                VALUES
                (1, 'testuser', 'password123', 2, true, 10, '[1, 2]', '{{}}'); -- testuser je završio encountere 1 i 2

            ";

            // Execute SQL script
            dbContext.Database.ExecuteSqlRaw(sqlScript);

            var level = 1;

            // Act
            var result = controller.GetTouristsByLevel(level);

            // Assert
            result.ShouldNotBeNull();
            result.Result.ShouldBeOfType<OkObjectResult>();
            sqlScript = @"
                DELETE FROM encounters.tourist_profiles;
            ";

            // Execute SQL script
            dbContext.Database.ExecuteSqlRaw(sqlScript);
        }

        [Fact]
        public void GetCompletedEncounters_Works()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();

            // Directly execute SQL queries to insert data
            string sqlScript = @"
                DELETE FROM encounters.tourist_profiles;
                INSERT INTO encounters.tourist_profiles
                (""Id"", ""Username"", ""PasswordHash"", ""Role"", ""IsActive"", ""XP"", ""CompletedEncountersIds"", ""CouponIds"")
                VALUES
                (1, 'testuser', 'password123', 2, true, 100, '[1, 2]', '{{}}'); -- testuser je završio encountere 1 i 2
            ";

            // Execute SQL script
            dbContext.Database.ExecuteSqlRaw(sqlScript);

            var touristId = 1;

            // Act
            var result = controller.GetCompletedEncounters(touristId);

            // Assert
            result.ShouldNotBeNull();
            result.Result.ShouldBeOfType<OkObjectResult>();

            var okResult = result.Result as OkObjectResult;
            var encounters = okResult.Value as IEnumerable<EncounterDTO>;

            encounters.ShouldNotBeNull();

            sqlScript = @"
                DELETE FROM encounters.tourist_profiles;
            ";

            // Execute SQL script
            dbContext.Database.ExecuteSqlRaw(sqlScript);
        }

        [Fact]
        public void GetTouristByUsername_Works()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
            var username = "testuser2";

            // Directly execute SQL queries to insert data
            string sqlScript = @"
                DELETE FROM encounters.tourist_profiles;
                INSERT INTO encounters.tourist_profiles
                (""Id"", ""Username"", ""PasswordHash"", ""Role"", ""IsActive"", ""XP"", ""CompletedEncountersIds"", ""CouponIds"")
                VALUES
                (-1, 'testuser2', 'password123', 2, true, 100, '[1, 2]', '{{}}'); -- testuser je završio encountere 1 i 2
            ";

            // Execute SQL script
            dbContext.Database.ExecuteSqlRaw(sqlScript);

            // Act
            var result = controller.GetTouristByUsername(username);

            // Assert
            result.ShouldNotBeNull();
            result.Result.ShouldBeOfType<OkObjectResult>();

            var okResult = result.Result as OkObjectResult;
            var tourist = okResult.Value as TouristProfileDTO;

            tourist.ShouldNotBeNull();
            tourist.Username.ShouldBe(username);

            sqlScript = @"
                DELETE FROM encounters.tourist_profiles;
            ";

            // Execute SQL script
            dbContext.Database.ExecuteSqlRaw(sqlScript);
        }



        [Fact]
        public void GetTouristByUsername_Fails_NotFound()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var username = "nonexistentuser";

            // Act
            var result = controller.GetTouristByUsername(username);

            // Assert
            result.ShouldNotBeNull();
            result.Result.ShouldBeOfType<NotFoundObjectResult>();

            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Value.ShouldBe($"Tourist with username '{username}' not found.");
        }

        private static TouristController CreateController(IServiceScope scope)
        {
            return new TouristController(scope.ServiceProvider.GetRequiredService<ITouristProfileService>());
        }
    }
}
