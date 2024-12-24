using Explorer.BuildingBlocks.Core.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.BuildingBlocks.Core.UseCases
{
    public interface IPaymentBundleService
    {
        Result<BundleDTO> Get(int bundleId);
        Result<List<BundleTourDTO>> GetAllTours(int bundleId);

    }
}
