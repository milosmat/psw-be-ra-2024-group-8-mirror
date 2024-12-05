using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Core.Domain.RepositoryInterfaces
{
    public interface ITouristProfileRepository : ICrudRepository<TouristProfile>
    {
        IEnumerable<TouristProfile> GetAll(Expression<Func<TouristProfile, bool>> filter = null);
        TouristProfile? GetWithIncludes(long id, params Expression<Func<TouristProfile, object>>[] includes);
        TouristProfile? GetByUsername(string username);
    }
}
