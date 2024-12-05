using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Payments.Core.Domain
{
    public class ShoppingCartBundle: Entity
    {

        public long BundleId { get; private set; } // ID paketa
        public string Name { get; private set; } // Naziv paketa
        public decimal Price { get; private set; } // Cena paketa
        public long ShoppingCartId { get; set; }

        public ShoppingCartBundle(long bundleId, string name, decimal price)
        {
            BundleId = bundleId;
            Name = name;
            Price = price;
        }

        public decimal TotalPrice => Price;
    }
}
