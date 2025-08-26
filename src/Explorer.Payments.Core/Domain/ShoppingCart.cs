using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Payments.Core.Domain
{
    public class ShoppingCart : Entity
    {
        public long TouristId { get; private set; }
        
        public List<ShoppingCartBundle>? ShopingBundles { get; private set; }
        public List<ShoppingCartItem> ShopingItems { get;  set; }


        public decimal TotalPrice { get; private set; }

        public int? ShopItemsCapacity { get; set; }
        public int? ShopBundlesCapacity { get; set; }

        public ShoppingCart(long touristId)
        {
            TouristId = touristId;
            ShopingItems = new List<ShoppingCartItem>();
            ShopingBundles = new List<ShoppingCartBundle>();
            TotalPrice = 0;
        }

        public decimal Totalprice => TotalPrice;

        public void AddItem(ShoppingCartItem item)
        {
            ShopingItems.Add(item);
            CalculateTotalPrice();
        }

        public void RemoveItem(ShoppingCartItem item)
        {
            if (ShopingItems.Remove(item))
            {
                CalculateTotalPrice();
            }
        }
        public void AddBundle(ShoppingCartBundle item)
        {
            ShopingBundles.Add(item);
            CalculateTotalPrice();
        }

        public void RemoveBundle(ShoppingCartBundle item)
        {
            if (ShopingBundles.Remove(item))
            {
                CalculateTotalPrice();
            }
        }

        public void UpdateItem(ShoppingCartItem itemToUpdate)
        {
            var existingItem = ShopingItems.FirstOrDefault(item => item.TourId == itemToUpdate.TourId);
            if (existingItem == null)
            {
                throw new KeyNotFoundException("Item not found.");
            }

            ShopingItems.FirstOrDefault(item => item.TourId == itemToUpdate.TourId).Price = itemToUpdate.Price;

        }

        public decimal CalculateTotalPrice()
        {
            TotalPrice = ShopingItems.Sum(item => item.TourPriceWithDiscount ?? item.Price) + ShopingBundles.Sum(bundle => bundle.Price);
            return TotalPrice;
        }

    }
}
