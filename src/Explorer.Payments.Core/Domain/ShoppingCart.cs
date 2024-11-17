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

        private decimal TotalPrice;

        public int? ShopItemsCapacity { get; set; }


        public ShoppingCart(long touristId)
        {
            TouristId = touristId;
            ShopingItems = new List<ShoppingCartItem>();
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

        private void UpdateTotalPrice()
        {
            TotalPrice = ShopingItems.Sum(item => item.TotalPrice);
        }

    }
}
