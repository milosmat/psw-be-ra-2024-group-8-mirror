using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Payments.Core.Domain
{
    public class ShoppingCartItem : Entity
    {

        public long TourId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public long ShoppingCartId { get; set; } // Foreign Key
        public decimal? TourPriceWithDiscount { get; set; }

        // Konstruktor sa ShoppingCartId
        public ShoppingCartItem() { }
        public ShoppingCartItem(long tourId, string name, decimal price)
        {
            TourId = tourId;
            Name = name;
            Price = price; 
        }
        public ShoppingCartItem(long tourId, string name, decimal price, decimal priceWithDiscount) 
        {
            TourId = tourId;
            Name = name;
            Price = price;
            TourPriceWithDiscount = priceWithDiscount;
        }


        public decimal? UpdateTourPrice(decimal newPrice)
        {
            TourPriceWithDiscount = newPrice;
            return TourPriceWithDiscount;
        }
        public decimal TotalPrice => Price;
    }
}

