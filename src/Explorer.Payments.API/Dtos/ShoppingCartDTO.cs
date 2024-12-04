using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Dtos
{
    public class ShoppingCartDTO
    {
        public int Id { get; set; }
        public long TouristId { get; set; }
        public List<ShoppingCartItemDTO> ShopingItems { get; set; } // Lista stavki u korpi

        public List<ShoppingBundleDto> ShopingBundles { get; set; } // Lista stavki u korpi

        public decimal TotalPrice { get; set; } // Ukupna cena korpe


        public class ShoppingCartItemDTO
        {
            public long Id { get; set; }
            public long TourId { get; set; } // ID ture
            public string TourName { get; set; } // Naziv ture
            public decimal TourPrice { get; set; } // Cena ture


        }

        public class ShoppingBundleDto
        {
            public long Id { get; set; }
            public long BundleId { get; set; } // ID ture
            public string Name { get; set; } // Naziv ture
            public decimal Price { get; set; } // Cena ture

        }

    }
}
