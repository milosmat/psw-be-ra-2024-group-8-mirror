using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Payments.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Payments.Infrastructure.Database.Repositories
{
    public class CardDataBaseRepository : CrudDatabaseRepository<ShoppingCart, PaymentsContext>, ICardRepository
    {
        public CardDataBaseRepository(PaymentsContext dbContext) : base(dbContext)
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
                .Include(t => t.ShopingBundles!)
                .FirstOrDefault();
        }


    }
}
