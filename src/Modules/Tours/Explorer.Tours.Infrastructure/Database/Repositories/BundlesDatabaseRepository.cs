using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Explorer.Tours.Core.UseCases.Author;
using Explorer.BuildingBlocks.Core.UseCases;


namespace Explorer.Tours.Infrastructure.Database.Repositories;

public class BundlesDatabaseRepository : CrudDatabaseRepository<Bundle, ToursContext>, IBundleRepository
{
    private readonly ICrudRepository<Tour> _tourRepository;

    public BundlesDatabaseRepository(ICrudRepository<Tour>tourRepository, ToursContext dbContext) : base(dbContext)
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

    public new Bundle? Get(long id)
    {
        return DbContext.Bundles
            .Include(b => b.Tours)
            .FirstOrDefault(b => b.Id == id);
    }

}
