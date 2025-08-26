using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.BuildingBlocks.Core.UseCases
{
    public interface ICouponSenderService
    {
        Result<string> GenerateCouponToNewUser(long userId);
    }
}
