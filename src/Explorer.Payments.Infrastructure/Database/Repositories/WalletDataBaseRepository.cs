using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Infrastructure.Database.Repositories
{
    public class WalletDataBaseRepository : CrudDatabaseRepository<Wallet, PaymentsContext>, IWalletRepository
    {
        public WalletDataBaseRepository(PaymentsContext dbContext) : base(dbContext)
        {
        }

        public new Wallet? Get(int id)
        {
            return DbContext.Wallets.Where(t=> t.Id == id).FirstOrDefault();
        }
    }
}
