using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.Core.Domain.Entities;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces
{
    public interface ITourPurchaseTokenRepository : ICrudRepository<TourPurchaseToken>
    {
    }
}
