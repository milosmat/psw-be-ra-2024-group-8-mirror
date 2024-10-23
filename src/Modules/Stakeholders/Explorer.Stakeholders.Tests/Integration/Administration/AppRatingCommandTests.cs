using Explorer.API.Controllers.Administrator.Administration;
using Explorer.API.Controllers.Tourist.Rating;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Tests.Integration.Administration
{
    [Collection("Sequential")]
    public class AppRatingCommandTests : BaseStakeholdersIntegrationTest
    {
        public AppRatingCommandTests(StakeholdersTestFactory factory) : base(factory)
        {
        }
        [Fact]
        public void Creates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.Migrate();
            DateTime currentTime = DateTime.UtcNow;
            var newEntity = new AppRatingDto
            {
                Rating = 3,
                Comment = "Could be more user frendly.",
                TimeCreated = currentTime,
                UserPostedId = -15
            };

            // Act
            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as AppRatingDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.TimeCreated.ShouldBe(currentTime);
            result.Comment.ShouldBe(newEntity.Comment);

            // Assert - Database
            var storedEntity = dbContext.AppRatings.FirstOrDefault(i => i.Comment == newEntity.Comment);
            storedEntity.ShouldNotBeNull();
            
        }
        private static AppRatingController CreateController(IServiceScope scope)
        {
            return new AppRatingController(scope.ServiceProvider.GetRequiredService<IAppRatingService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
    
}
