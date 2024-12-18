using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.API.Controllers.Tourist;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Author;
using Explorer.API.Controllers.Author;

namespace Explorer.Tours.Tests.Integration.Author
{
    [Collection("Sequential")]
    public class TourSaleControllerTests : BaseToursIntegrationTest
    {
        public TourSaleControllerTests(ToursTestFactory factory) : base(factory)
        {
        }

        [Fact]
        public void Creates_TourSale()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var newTourSale = new TourSaleDto
            {
                Tours = new List<int> { 1, 2, 3 },
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                Discount = 15.0,
                Active = true,
                AuthorId = 123
            };

            var result = ((ObjectResult)controller.Create(newTourSale).Result)?.Value as TourSaleDto;

            result.ShouldNotBeNull();
            result.Tours.ShouldBeEquivalentTo(newTourSale.Tours);
            result.StartDate.ShouldBe(newTourSale.StartDate);
            result.EndDate.ShouldBe(newTourSale.EndDate);
            result.Discount.ShouldBe(newTourSale.Discount);
            result.Active.ShouldBe(newTourSale.Active);
            result.AuthorId.ShouldBe(newTourSale.AuthorId);

            var storedTourSale = dbContext.TourSales.FirstOrDefault(ts => ts.AuthorId == result.AuthorId && ts.StartDate == result.StartDate);
            storedTourSale.ShouldNotBeNull();
        }

        [Fact]
        public void Create_Fails_Invalid_Data()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var invalidTourSale = new TourSaleDto
            {
                Tours = new List<int>(), 
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(-1), 
                Discount = -5.0, 
                Active = true,
                AuthorId = 0 
            };

            var result = (ObjectResult)controller.Create(invalidTourSale).Result;

            result.StatusCode.ShouldBe(400);
        }

        [Fact]
        public void Deletes_TourSale()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var result = controller.Delete(1); 

            var okResult = result as OkResult;
            okResult.ShouldNotBeNull();
            okResult.StatusCode.ShouldBe(200);

            var storedTourSale = dbContext.TourSales.FirstOrDefault(ts => ts.Id == 1);
            storedTourSale.ShouldBeNull();
        }

        [Fact]
        public void Delete_Fails_Invalid_Id()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var result = (ObjectResult)controller.Delete(-1); // Invalid Id

            result.StatusCode.ShouldBe(404);
        }

        [Fact]
        public void Updates_TourSale()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var existingTourSale = dbContext.TourSales.FirstOrDefault(ts => ts.Id == 1);
            existingTourSale.ShouldNotBeNull();

            var updatedTourSaleDto = new TourSaleDto
            {
                Id = 1, 
                Tours = new List<int> { 4, 5, 6 }, 
                StartDate = existingTourSale.StartDate,
                EndDate = existingTourSale.EndDate.AddDays(5), 
                Discount = 20.0, 
                Active = false, 
                AuthorId = existingTourSale.AuthorId
            };

            var result = ((ObjectResult)controller.Update(updatedTourSaleDto).Result)?.Value as TourSaleDto;

            result.ShouldNotBeNull();
            result.Tours.ShouldBeEquivalentTo(updatedTourSaleDto.Tours);
            result.EndDate.ShouldBe(updatedTourSaleDto.EndDate);
            result.Discount.ShouldBe(updatedTourSaleDto.Discount);
            result.Active.ShouldBe(updatedTourSaleDto.Active);

            var storedTourSale = dbContext.TourSales.FirstOrDefault(ts => ts.Id == updatedTourSaleDto.Id);
            storedTourSale.ShouldNotBeNull();
            storedTourSale.Tours.ShouldBeEquivalentTo(updatedTourSaleDto.Tours);
            storedTourSale.EndDate.ShouldBe(updatedTourSaleDto.EndDate);
            storedTourSale.Discount.ShouldBe(updatedTourSaleDto.Discount);
            storedTourSale.Active.ShouldBe(updatedTourSaleDto.Active);
        }

        [Fact]
        public void Update_Fails_Invalid_Id()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var invalidTourSaleDto = new TourSaleDto
            {
                Id = -1, 
                Tours = new List<int> { 4, 5 },
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                Discount = 10.0,
                Active = true,
                AuthorId = 123
            };

            var result = (ObjectResult)controller.Update(invalidTourSaleDto).Result;

            result.StatusCode.ShouldBe(404);
        }

        private static TourSaleController CreateController(IServiceScope scope)
        {
            return new TourSaleController(scope.ServiceProvider.GetRequiredService<ITourSaleService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
