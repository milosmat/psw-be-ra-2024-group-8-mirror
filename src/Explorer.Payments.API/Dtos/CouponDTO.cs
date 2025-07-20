using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Dtos
{
    public class CouponDTO
    {
        public int Id { get; set; } 
        public string Code { get; set; } // Nasumično generisan kod (8 karaktera)
        public int DiscountPercentage { get; set; } // Procenat popusta
        public DateTime? ExpiryDate { get; set; } // Datum do kada važi (opciono)
        public long? TourId { get; set; } // ID ture, null ako važi za sve ture autora
        public long? AuthorId { get; set; }
        public long? RecipientId { get; set; }
        public bool IsPublic { get; set; }

    }
}
