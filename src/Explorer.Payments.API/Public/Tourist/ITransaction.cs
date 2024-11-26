using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Explorer.Payments.API.Public.Tourist
{
    public interface ITransaction
    {
        Result<Transaction> Get(int id);
    }
}
