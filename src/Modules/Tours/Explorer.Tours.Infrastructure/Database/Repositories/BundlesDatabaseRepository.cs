using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Infrastructure.Database.Repositories;


public class BundlesDatabaseRepository : CrudDatabaseRepository<Bundle, ToursContext>, IBundleRepository
{
    private readonly ICrudRepository<Tour> _tourRepository;

    public BundlesDatabaseRepository(ICrudRepository<Tour> tourRepository, ToursContext dbContext) : base(dbContext)
    {
        _tourRepository = tourRepository;
    }

    public List<Bundle> GetPublishedBundles()
    {
        return DbContext.Bundles
            .Include(b => b.Tours)
            .Where(b => b.Status == BundleStatus.PUBLISHED)
            .ToList();
    }

    public List<Bundle> GetAllBundles()
    {
        return DbContext.Bundles
            .Include(b => b.Tours)
            .ToList();
    }

    public new Bundle? Get(long id)
    {
        return DbContext.Bundles.Where(t => t.Id == id)
            .Include(b => b.Tours)
            .FirstOrDefault();
    }

}
