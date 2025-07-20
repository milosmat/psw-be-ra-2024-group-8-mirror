using Explorer.API.Controllers.Tourist.Shopping;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public.Tourist;
using Explorer.Payments.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Tests.Integration
{
    [Collection("Sequential")]
    public class ShoppingCartCommandTests : BasePaymentsIntegrationTest
    {
        public ShoppingCartCommandTests(PaymentsTestFactory factory) : base(factory) { }

        [Theory]
        [InlineData(2, 1, "Tour A", 150.00)]
        [InlineData(2, 2, "Tour B", 200.00)]
        public void AddMultipleToursToExistingTouristCart_Successfully(long touristId, long tourId, string tourName, decimal tourPrice)
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

            var newOrderItem = new ShoppingCartItemDto
            {
                TourId = tourId,
                TourName = tourName,
                TourPrice = tourPrice
            };

            // Act
            var result = ((ObjectResult)controller.AddTourToCart(touristId, newOrderItem).Result)?.Value as ShoppingCartItemDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.TourId.ShouldBe(tourId);
            result.TourName.ShouldBe(tourName);
            result.TourPrice.ShouldBe(tourPrice);

            //Assert - Database
            var storedEntity = dbContext.ShoppingCartItems.FirstOrDefault(item => item.Id == result.Id);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldBe(result.Id);
        }


        [Theory]
        [InlineData(1, 1, "Tour A", 150.00)]
        public void CreateNewCartForTourist_And_AddTour_WhenCartDoesNotExist_Successfully(long touristId, long tourId, string tourName, decimal tourPrice)
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

            var newOrderItem = new ShoppingCartItemDto
            {
                TourId = tourId,
                TourName = tourName,
                TourPrice = tourPrice
            };

            // Act
            var result = ((ObjectResult)controller.AddTourToCart(touristId, newOrderItem).Result)?.Value as ShoppingCartItemDto;

            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.TourId.ShouldBe(tourId);
            result.TourName.ShouldBe(tourName);
            result.TourPrice.ShouldBe(tourPrice);

            // Assert: Verify cart creation and database
            var storedCart = dbContext.ShoppingCard.FirstOrDefault(cart => cart.TouristId == touristId);
            storedCart.ShouldNotBeNull();

            var storedCartForTourist = dbContext.ShoppingCard.Count(cart => cart.TouristId == touristId);
            storedCartForTourist.ShouldBe(1);

            var storedEntity = dbContext.ShoppingCartItems.FirstOrDefault(item => item.Id == result.Id);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldBe(result.Id);
        }


        [Theory]
        [InlineData(3, 3, "TourToRemove", 20.00)]
        public void RemoveSingleTourFromCart_Successfully(long touristId, long tourId, string tourName, decimal tourPrice)
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

            var newOrderItem = new ShoppingCartItemDto
            {
                TourId = tourId,
                TourName = tourName,
                TourPrice = tourPrice
            };
            var result = ((ObjectResult)controller.AddTourToCart(touristId, newOrderItem).Result)?.Value as ShoppingCartItemDto;

            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.TourId.ShouldBe(tourId);
            result.TourName.ShouldBe(tourName);

            // Act: Remove method
            controller.RemoveTourFromCart(touristId, result.TourId);

            //Assert
            var postDeleteOrderItem = dbContext.ShoppingCartItems.FirstOrDefault(item => item.Id == result.Id);
            postDeleteOrderItem.ShouldBeNull();

            //Assert: check the cart exisitng
            var storedCartForTourist = dbContext.ShoppingCard.FirstOrDefault(cart => cart.TouristId == touristId);
            storedCartForTourist.ShopingItems.Count.ShouldBe(0);
        }

        [Theory]
        [InlineData(4, 3, "TourToRemove", 20.00, 1, "Tour A", 150.00)]
        public void RemoveTourFromCartWithOtherItems_Successfully(long touristId, long tourToRemoveId, string tourToRemoveName, decimal tourToRemovePrice,
            long tourId, string tourName, decimal tourPrice)
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

            var newOrderItem = new ShoppingCartItemDto
            {
                TourId = tourId,
                TourName = tourName,
                TourPrice = tourPrice
            };
            var result = ((ObjectResult)controller.AddTourToCart(touristId, newOrderItem).Result)?.Value as ShoppingCartItemDto;

            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.TourId.ShouldBe(tourId);
            result.TourName.ShouldBe(tourName);

            var newOrderItemToRemove = new ShoppingCartItemDto
            {
                TourId = tourToRemoveId,
                TourName = tourToRemoveName,
                TourPrice = tourToRemovePrice
            };
            var resultToRemove = ((ObjectResult)controller.AddTourToCart(touristId, newOrderItemToRemove).Result)?.Value as ShoppingCartItemDto;

            resultToRemove.ShouldNotBeNull();
            resultToRemove.Id.ShouldNotBe(0);
            resultToRemove.TourId.ShouldBe(tourToRemoveId);
            resultToRemove.TourName.ShouldBe(tourToRemoveName);

            // Act: Remove method
            controller.RemoveTourFromCart(touristId, resultToRemove.TourId);

            //Assert
            var postDeleteOrderItem = dbContext.ShoppingCartItems.FirstOrDefault(item => item.Id == resultToRemove.Id);
            postDeleteOrderItem.ShouldBeNull();

            //Assert: check the cart exisitng
            var storedCartForTourist = dbContext.ShoppingCard.FirstOrDefault(cart => cart.TouristId == touristId);
            storedCartForTourist.ShopingItems.Count.ShouldBe(1);
        }



        private static ShoppingCardController CreateController(IServiceScope scope)
        {
            return new ShoppingCardController(
                scope.ServiceProvider.GetRequiredService<IShoppingCartService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }

    }
}

