using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Infrastructure.Database.Repositories
{
    public class CouponDataBaseRepository : CrudDatabaseRepository<Coupon, PaymentsContext>, ICouponRepository
    {
        public CouponDataBaseRepository(PaymentsContext dbContext) : base(dbContext)
        {
        }

        public new Coupon? Get(int id)
        {
            return DbContext.Coupons.Where(c => c.Id == id)
                .FirstOrDefault();
        }

        public Coupon Update(Coupon aggregateRoot)
        {
            DbContext.Entry(aggregateRoot).State = EntityState.Modified;
            DbContext.SaveChanges();
            return aggregateRoot;
        }

        public List<Coupon> GetCouponsByCode(string code)
        {
            return DbContext.Coupons.Where(c => c.Code == code).ToList();
        }

        public IEnumerable<Coupon> GetAll()
        {
            return DbContext.Coupons.ToList();
        }
    }
}
