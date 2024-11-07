using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.Core.Domain.Entities;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Tours.Infrastructure.Database.Repositories
{
    public class TourPurchaseTokenRepository : CrudDatabaseRepository<TourPurchaseToken, ToursContext>, ITourPurchaseTokenRepository
    {
        public TourPurchaseTokenRepository(ToursContext dbContext) : base(dbContext)
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
                .Where(t => t.TouristId == touristId && t.Status == TourPurchaseToken.TokenStatus.Active)
                .FirstOrDefault();
        }
    }
}
