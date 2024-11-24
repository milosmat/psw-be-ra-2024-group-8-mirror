using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;

namespace Explorer.Payments.Infrastructure.Database.Repositories
{
    public class WalletDataBaseRepository : CrudDatabaseRepository<Wallet, PaymentsContext>, IWalletRepository
    {
        public WalletDataBaseRepository(PaymentsContext dbContext) : base(dbContext)
        {
        }

        public new Wallet? Get(int id)
        {
            return DbContext.Wallets.Where(t => t.Id == id).FirstOrDefault();
        }

        public Wallet GetWallet(int id)
        {
            Wallet? wallet = DbContext.Wallets.Where(t => t.Id == id).FirstOrDefault();

            if (wallet == null)
            {
                return null;
            }

            List<Transaction>? transactions = DbContext.Transactions.Where(t => t.WalletId == id).ToList();

            wallet.Transactions = transactions;


            return wallet;
        }
    }
}
