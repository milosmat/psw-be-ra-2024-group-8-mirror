using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces;

public interface IBundleRepository : ICrudRepository<Bundle>
{
    List<Bundle> GetPublishedBundles();
    List<Bundle> GetAllBundles();
    Bundle Create(Bundle newBundle);

    List<BundleTour> GetAllByBundleId(int bundleId);


}
