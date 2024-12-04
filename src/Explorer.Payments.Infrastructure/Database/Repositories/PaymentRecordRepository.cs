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
    public class PaymentRecordRepository : CrudDatabaseRepository<PaymentRecord, PaymentsContext>, IPaymentRecordRepository
    {
        public PaymentRecordRepository(PaymentsContext dbContext) : base(dbContext)
        {
        }

        public List<PaymentRecord> GetAllByTouristId(long touristId)
        {
            // Pretražuje PaymentRecords tabelu po touristId
            var paymentRecords = DbContext.PaymentRecord
                                         .Where(record => record.TouristId == touristId)
                                         .ToList();

            return paymentRecords;
        }




    }
}
