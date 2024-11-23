using Explorer.BuildingBlocks.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Explorer.Payments.Core.Domain
{
    public class Wallet : Entity
    {
        public long TouristId { get; private set; }
        public long AdventureCoins { get; set; }
        public List<Transaction> Transactions { get; set; }

        public Wallet() { }
        public Wallet(long touristId)
        {
            TouristId = touristId;
            AdventureCoins = 0;
            Transactions = null;
        }


        //Metode za upravljanje Transaction
        public Result AddTransaction(long administratorId, long adventureCoins, String description) {
            
            if (adventureCoins < 0)
                return Result.Fail("AdventureCoins lover then 0");

            Transactions.Add(new Transaction(adventureCoins, description, DateTime.Now, administratorId));
            this.AdventureCoins += adventureCoins;


            return Result.Ok();
        }
        public Result SubtractTransaction(long administratorId, long adventureCoins, String description)
        {
            if (adventureCoins < 0)
                return Result.Fail("AdventureCoins lover then 0");

            if ((this.AdventureCoins - adventureCoins) < 0)
                return Result.Fail("The tourist does not have enough Adventure Coins for this transaction");

            Transactions.Add(new Transaction(adventureCoins, description, DateTime.Now, administratorId));

            this.AdventureCoins -= adventureCoins;

            return Result.Ok();
        }

    }
}
