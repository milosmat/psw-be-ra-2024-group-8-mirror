using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain
{
    public class ShoppingCartItem : Entity
    {
        public long TourId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public long ShoppingCartId { get; set; } // Foreign Key

        // Konstruktor sa ShoppingCartId
        public ShoppingCartItem(long tourId, string name, decimal price)
        {
            TourId = tourId;
            Name = name;
            Price = price;
        }

        public decimal TotalPrice => Price;
    }
}
