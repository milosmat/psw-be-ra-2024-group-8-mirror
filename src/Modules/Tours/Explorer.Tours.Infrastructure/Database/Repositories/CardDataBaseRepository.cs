using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Tours.Infrastructure.Database.Repositories
{
    public class CardDataBaseRepository : CrudDatabaseRepository<ShoppingCart, ToursContext>, ICardRepository
    {
        public CardDataBaseRepository(ToursContext dbContext) : base(dbContext)
        {
        }

        public new ShoppingCart? Get(long id)
        {
            return DbContext.ShoppingCard.Where(t => t.Id == id)
                .Include(t => t.ShopingItems!).FirstOrDefault();
        }

        public ShoppingCart Update(ShoppingCart aggregateRoot)
        {
            DbContext.Entry(aggregateRoot).State = EntityState.Modified;
            DbContext.SaveChanges();
            return aggregateRoot;
        }


        public ShoppingCart? GetByTouristId(long touristId)
        {
            return DbContext.ShoppingCard
                .Where(t => t.TouristId == touristId)  // Traži po TouristId
                .Include(t => t.ShopingItems!)
                .FirstOrDefault();
        }

    }
}
