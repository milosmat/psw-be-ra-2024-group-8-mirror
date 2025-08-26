using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain
{
    public class Coupon : Entity
    {
        public string Code { get; private set; } // Nasumično generisan kod (8 karaktera)
        public int DiscountPercentage { get; private set; } // Procenat popusta
        public DateTime? ExpiryDate { get; private set; } // Datum do kada važi (opciono)
        public long? TourId { get; private set; } // ID ture, null ako važi za sve ture autora
        public long?AuthorId { get; private set; } // Autor kupona
        public long? RecipientId { get; private set;}
        public bool IsPublic { get; set; } = false;


        public Coupon() { }

        public Coupon(int id,string code, int discountPercentage, bool isPublic, DateTime? expiryDate, long? tourId, long? authorId, long? recipientId)
        { 
            if (discountPercentage <= 0) throw new ArgumentException("Discount Percentage must be positive.", nameof(discountPercentage));

            Id = id;
            Code = code;
            DiscountPercentage = discountPercentage;
            ExpiryDate = expiryDate;
            TourId = tourId;
            AuthorId = authorId;
            RecipientId = recipientId;
            IsPublic = isPublic;
        }

        public Coupon(string code, int discountPercentage, bool isPublic, DateTime? expiryDate, long? tourId, long? authorId, long? recipientId)
        {
            if (discountPercentage <= 0) throw new ArgumentException("Discount Percentage must be positive.", nameof(discountPercentage));

            Code = code;
            DiscountPercentage = discountPercentage;
            ExpiryDate = expiryDate;
            TourId = tourId;
            AuthorId = authorId;
            RecipientId = recipientId;
            IsPublic = isPublic;
        }
       
    }
}
