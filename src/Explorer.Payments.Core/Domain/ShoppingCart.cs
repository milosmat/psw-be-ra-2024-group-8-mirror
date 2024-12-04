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
        public List<ShoppingCartItem> ShopingItems { get; private set; }
        public List<ShoppingCartBundle>? ShopingBundles { get; private set; }


        private decimal TotalPrice;

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
            UpdateTotalPrice();
        }

        public void RemoveItem(ShoppingCartItem item)
        {
            if (ShopingItems.Remove(item))
            {
                UpdateTotalPrice();
            }
        }
        public void AddBundle(ShoppingCartBundle item)
        {
            ShopingBundles.Add(item);
            UpdateTotalPrice();
        }

        public void RemoveBundle(ShoppingCartBundle item)
        {
            if (ShopingBundles.Remove(item))
            {
                UpdateTotalPrice();
            }
        }

        private void UpdateTotalPrice()
        {
            TotalPrice = ShopingItems.Sum(item => item.TotalPrice) + ShopingBundles.Sum(bundle => bundle.Price);
        }

    }
}
