using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Dtos
{
    public class ShoppingCartItemDto
    {
        public long Id { get; set; }
        public long TourId { get; set; } // ID ture
        public string TourName { get; set; } // Naziv ture
        public decimal TourPrice { get; set; } // Cena ture
        //public long ShoppingCartId { get; set; }


    }
}
