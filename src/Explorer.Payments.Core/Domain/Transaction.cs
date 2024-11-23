using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain
{
    public class Transaction : Entity
    {
        public long Amount { get; set; } // iznos AC koji treba da se uplati/skine sa walleta
        public String Description { get; set; } //Poruka koja prati transakciju
        public DateTime TransactionTime { get; set; }
        public long AdministratorId { get; set; } //Administrator koji vrsi transakciju
        public long WalletId {  get; set; }

        public Transaction(long amount, string description, DateTime transactionTime, long administratorId, long walletId)
        {
            Amount = amount;
            Description = description;
            TransactionTime = transactionTime;
            AdministratorId = administratorId;
            WalletId = walletId;
        }
    }
}
