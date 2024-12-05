using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Payments.API.Public.Tourist;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Explorer.Payments.Infrastructure.Database.Repositories
{
    public class TransactionDataBaseRepository : CrudDatabaseRepository<Transaction, PaymentsContext>, ITransactionRepository
    {

        public TransactionDataBaseRepository(PaymentsContext dbContext) : base(dbContext)
        {
        }
        
        

        public List<Transaction>? GetTranaction(int id) //pretraga po ID walleta
        {
            return DbContext.Transactions.Where(t => t.WalletId == id).ToList();
        }

        public List<Transaction> GetTransactions(int id)
        {
            return DbContext.Transactions.Where(t => t.WalletId == id).ToList();
        }
    }
}
