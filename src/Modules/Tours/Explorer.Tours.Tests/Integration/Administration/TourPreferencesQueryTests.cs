using Explorer.API.Controllers.Administrator.Administration;
using Explorer.API.Controllers.Tourist;
using Explorer.BuildingBlocks.Core.UseCases;
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
using Microsoft.EntityFrameworkCore;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using Explorer.Stakeholders.API.Dtos;

namespace Explorer.Tours.Tests.Integration.Administration
{
    [Collection("Sequential")]
    public class TourPreferencesQueryTests : BaseToursIntegrationTest
    {
        public TourPreferencesQueryTests(ToursTestFactory factory) : base(factory)
        {
        }

        [Theory]
        [InlineData(0, 3, 3)]  // Prva stranica sa 3 komentara
        public void Retrieves_all(int pageNumber, int pageSize, int expectedResultsCount)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = ((ObjectResult)controller.GetAll(pageNumber, pageSize).Result)?.Value as PagedResult<TourPreferencesDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(expectedResultsCount);
            result.TotalCount.ShouldBeGreaterThanOrEqualTo(expectedResultsCount);
        }

        private static TourPreferencesController CreateController(IServiceScope scope)
        {
            return new TourPreferencesController(scope.ServiceProvider.GetRequiredService<ITourPreferenceService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }

    }
}
