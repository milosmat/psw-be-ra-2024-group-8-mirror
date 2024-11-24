using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Transaction = Explorer.Payments.Core.Domain.Transaction;


namespace Explorer.Payments.API.Dtos
{
    public class WalletDTO
    {
        public int Id { get; set; }
        public long TouristId { get; set; }
        public long AdventureCoins {  get; set; }
        public List<TransactionItemsDTO>? Transactions { get; set; }


        public class TransactionItemsDTO
        {
            public long Amount { get; set; }
            public String Description { get; set; } 
            public DateTime TransactionTime { get; set; }
            public long AdministratorId { get; set; }

            public TransactionItemsDTO(long amount, string description, DateTime transactionTime, long administratorId)
            {
                Amount = amount;
                Description = description;
                TransactionTime = transactionTime;
                AdministratorId = administratorId;
            }
        }

    }
}
