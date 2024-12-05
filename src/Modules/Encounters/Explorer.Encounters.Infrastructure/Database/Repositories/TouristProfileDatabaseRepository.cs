using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Explorer.Encounters.Infrastructure.Database.Repositories
{
    public class TouristProfileDatabaseRepository : CrudDatabaseRepository<TouristProfile, EncountersContext>, ITouristProfileRepository
    {
        public TouristProfileDatabaseRepository(EncountersContext dbContext) : base(dbContext)
        {
        }
        public TouristProfile? GetByUsername(string username)
        {
            return DbContext.TouristProfiles.FirstOrDefault(tp => tp.Username == username);
        }
        public IEnumerable<TouristProfile> GetAll(Expression<Func<TouristProfile, bool>> filter = null)
        {
            var query = DbContext.Set<TouristProfile>().AsQueryable();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.ToList();
        }

        public TouristProfile? GetWithIncludes(long id, params Expression<Func<TouristProfile, object>>[] includes)
        {
            var query = DbContext.Set<TouristProfile>().AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.FirstOrDefault(t => t.Id == id);
        }
    }
}
