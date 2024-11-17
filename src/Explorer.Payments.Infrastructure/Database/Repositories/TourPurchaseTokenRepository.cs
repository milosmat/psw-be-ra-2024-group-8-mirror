using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Payments.Core.Domain;
using Microsoft.EntityFrameworkCore;
using static Explorer.Payments.Core.Domain.TourPurchaseToken;

namespace Explorer.Payments.Infrastructure.Database.Repositories
{
    public class TourPurchaseTokenRepository : CrudDatabaseRepository<TourPurchaseToken, PaymentsContext>, ITourPurchaseTokenRepository
    {
        public TourPurchaseTokenRepository(PaymentsContext dbContext) : base(dbContext)
        {
        }

        public new TourPurchaseToken? Get(long id)
        {
            return DbContext.Tokens
                .Where(t => t.Id == id)
                .FirstOrDefault();
        }

        public TourPurchaseToken Update(TourPurchaseToken aggregateRoot)
        {
            DbContext.Entry(aggregateRoot).State = EntityState.Modified;
            DbContext.SaveChanges();
            return aggregateRoot;
        }

        public TourPurchaseToken? GetActiveTokenByTourist(long touristId)
        {
            return DbContext.Tokens
                .Where(t => t.TouristId == touristId && t.Status == TokenStatus.Active)
                .FirstOrDefault();
        }

        public IEnumerable<TourPurchaseToken> GetAllTokens()
        {
            return DbContext.Tokens.ToList();
        }

    }
}
