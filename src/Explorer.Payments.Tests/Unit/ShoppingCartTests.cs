using Explorer.Payments.API.Dtos;
using Explorer.Payments.Core.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Tests.Unit
{
    public class ShoppingCartTests
    {
        [Fact]
        public void AddItemToCart_CheckTheContentOfTheCart()
        {
            //Arrange
            var shoppingCart = new ShoppingCart(1);
            var orderItem = new ShoppingCartItem(1, "Tour 1", 10);

            //Act
            shoppingCart.AddItem(orderItem);

            //Assert
            Assert.Contains(orderItem, shoppingCart.ShopingItems);
            Assert.Single(shoppingCart.ShopingItems); 
        }

        [Fact]
        public void RemoveItemFromCart_CheckTheContent_TotalPriceUpdated()
        {
            //Arrange
            var shoppingCart = new ShoppingCart(1);
            var orderItem = new ShoppingCartItem(1, "Tour 1", 10);
            shoppingCart.AddItem(orderItem);

            //Act
            shoppingCart.RemoveItem(orderItem);

            //Assert
            Assert.DoesNotContain(orderItem, shoppingCart.ShopingItems);
            Assert.Equal(0, shoppingCart.Totalprice);
        }

        [Fact]
        public void CalculateTotalPrice_NoItems_ReturnZero()
        {
            //Arrange
            var shoppingCart = new ShoppingCart(1);

            //Act
            var totalPrice = shoppingCart.CalculateTotalPrice();

            //Assert
            Assert.Equal(0, totalPrice);
        }

        [Fact]
        public void CalculateTotalPrice_SingleItem_ReturnItemPrice()
        {
            //Arrange
            var shoppingCart = new ShoppingCart(1);
            var orderItem = new ShoppingCartItem(1, "Tour A", 150);
            shoppingCart.AddItem(orderItem);

            //Act
            var totalPrice = shoppingCart.CalculateTotalPrice();

            //Assert
            Assert.Equal(orderItem.Price, totalPrice);
        }

        [Fact]
        public void CalculateTotalPrice_SingleItemForFree_ReturnZero()
        {
            //Arrange
            var shoppingCart = new ShoppingCart(1);
            var orderItem = new ShoppingCartItem(1, "Tour A", 0);
            shoppingCart.AddItem(orderItem);

            //Act
            var totalPrice = shoppingCart.CalculateTotalPrice();

            //Assert
            Assert.Equal(orderItem.Price, totalPrice);
        }

        [Fact]
        public void CalculateTotalPrice_MultipleItems_ReturnSumOfPrices()
        {
            //Arrange
            var shoppingCart = new ShoppingCart(1);
            var orderItem1 = new ShoppingCartItem(1, "Tour A", 150);
            var orderItem2 = new ShoppingCartItem(2, "Tour B", 200);
            shoppingCart.AddItem(orderItem1);
            shoppingCart.AddItem(orderItem2);

            //Act
            var totalPrice = shoppingCart.CalculateTotalPrice();

            //Assert
            Assert.Equal(orderItem1.Price + orderItem2.Price, totalPrice);
        }

        [Fact]
        public void CalculateTotalPrice_ItemsAndBundles_ReturnSumOfPrices()
        {
            //Arrange
            var shoppingCart = new ShoppingCart(1); 
            var orderItem1 = new ShoppingCartItem(1, "Tour A", 150);
            var orderItem2 = new ShoppingCartItem(2, "Tour B", 200);
            var bundle = new ShoppingCartBundle(1, "Boundle A", 500);

            shoppingCart.AddItem(orderItem1); 
            shoppingCart.AddItem(orderItem2); 
            shoppingCart.AddBundle(bundle);

            //Act
            var totalPrice = shoppingCart.CalculateTotalPrice();

            //Assert
            Assert.Equal(orderItem1.Price + orderItem2.Price + bundle.Price, totalPrice);
        }
    }
}
