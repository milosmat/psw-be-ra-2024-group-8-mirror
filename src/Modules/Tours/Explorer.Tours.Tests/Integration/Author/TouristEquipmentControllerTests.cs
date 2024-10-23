using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Explorer.API.Controllers;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.Infrastructure.Database;
using Explorer.API.Controllers.Author;
using Explorer.Tours.API.Public.Author;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.API.Controllers.Tourist;
using Explorer.Tours.API.Public.Tourist;

namespace Explorer.Tours.Tests.Integration.Author
{
    [Collection("Sequential")]
    public class TouristEquipmentControllerTests : BaseToursIntegrationTest
    {
        public TouristEquipmentControllerTests(ToursTestFactory factory) : base(factory)
        {
        }

        [Fact]
        public void Creates_TouristEquipment()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var newTouristEquipment = new TouristEquipmentDTO
            {
                TouristId = 5,
                EquipmentId = 10
            };

            // Act
            var result = ((ObjectResult)controller.Create(newTouristEquipment).Result)?.Value as TouristEquipmentDTO;

            // Assert - Response
            result.ShouldNotBeNull();
            result.TouristId.ShouldBe(newTouristEquipment.TouristId);
            result.EquipmentId.ShouldBe(newTouristEquipment.EquipmentId);

            // Assert - Database
            var storedTouristEquipment = dbContext.TouristEquipments
                .FirstOrDefault(te => te.TouristId == result.TouristId && te.EquipmentId == result.EquipmentId);
            storedTouristEquipment.ShouldNotBeNull();
        }

        [Fact]
        public void Create_Fails_Invalid_Data()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var invalidTouristEquipment = new TouristEquipmentDTO
            {
                TouristId = 0,  // Invalid TouristId
                EquipmentId = 2
            };

            // Act
            var result = (ObjectResult)controller.Create(invalidTouristEquipment).Result;

            // Assert
            result.StatusCode.ShouldBe(400);
        }

        /*[Fact]
        public void Updates_TouristEquipment()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var updatedTouristEquipment = new TouristEquipmentDTO
            {
                TouristId = 1,
                EquipmentId = 3 // Changing EquipmentId
            };

            // Act
            var result = ((ObjectResult)controller.Update(updatedTouristEquipment).Result)?.Value as TouristEquipmentDTO;

            // Assert
            result.ShouldNotBeNull();
            result.EquipmentId.ShouldBe(updatedTouristEquipment.EquipmentId);

            var storedTouristEquipment = dbContext.TouristEquipments
                .FirstOrDefault(te => te.TouristId == updatedTouristEquipment.TouristId);
            storedTouristEquipment.ShouldNotBeNull();
            storedTouristEquipment.EquipmentId.ShouldBe(updatedTouristEquipment.EquipmentId);
        }

        [Fact]
        public void Update_Fails_Invalid_Id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var invalidTouristEquipment = new TouristEquipmentDTO
            {
                TouristId = -100,  // Invalid TouristId
                EquipmentId = 2
            };

            // Act
            var result = (ObjectResult)controller.Update(invalidTouristEquipment).Result;

            // Assert
            result.StatusCode.ShouldBe(404);
        }*/

        [Fact]
        public void Deletes_TouristEquipment()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            // Act
            var result = controller.Delete(1);  // Assuming Id = 1 exists

            // Assert
            var okResult = result as OkResult;
            okResult.ShouldNotBeNull();
            okResult.StatusCode.ShouldBe(200);

            var storedTouristEquipment = dbContext.TouristEquipments
                .FirstOrDefault(te => te.Id == 1);
            storedTouristEquipment.ShouldBeNull();
        }

        [Fact]
        public void Delete_Fails_Invalid_Id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = (ObjectResult)controller.Delete(-1);  // Invalid Id

            // Assert
            result.StatusCode.ShouldBe(404);
        }

        private static TouristEquipmentController CreateController(IServiceScope scope)
        {
            return new TouristEquipmentController(scope.ServiceProvider.GetRequiredService<ITouristEquipmentService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
