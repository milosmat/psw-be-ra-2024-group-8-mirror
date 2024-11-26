using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain.RepositoryInterfaces
{
    public interface ITransactionRepository : ICrudRepository<Transaction>
    {
        List<Transaction> GetTransactions(int id); 
    }
}
