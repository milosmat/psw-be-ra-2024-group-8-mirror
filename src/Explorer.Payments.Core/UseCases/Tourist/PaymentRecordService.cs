using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Payments.API.Public.Tourist;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;

namespace Explorer.Payments.Core.UseCases.Tourist
{
    public class PaymentRecordService : IPaymentRecordService
    {
        private readonly IPaymentRecordRepository _paymentRepository;

        public PaymentRecordService(IPaymentRecordRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

    }
}
