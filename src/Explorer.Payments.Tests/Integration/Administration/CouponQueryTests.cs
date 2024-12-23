using Explorer.API.Controllers.Author;
using Explorer.Payments.API.Public.Tourist;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Tests.Integration.Administration
{
    [Collection("Sequential")]
    public class CouponQueryTests : BasePaymentsIntegrationTest
    {
        public CouponQueryTests(PaymentsTestFactory factory) : base(factory) { }

        private static CouponController CreateController(IServiceScope scope)
        {
            return new CouponController(scope.ServiceProvider.GetRequiredService<ICouponService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }

    }


}
