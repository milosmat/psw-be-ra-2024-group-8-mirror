using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.BuildingBlocks.Core.UseCases
{
    public interface IEmailService
    {
        Task<Result> SendEmailToUserAsync(string email, string subject, string body);
        Task<Result> SendNewCouponNotificatinToAllTourists(string subject, string body);
    }
}
