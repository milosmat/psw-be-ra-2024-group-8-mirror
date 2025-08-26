using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Games.Core.Domain.RepositoryInterfaces
{
    public interface IGamesRepository : ICrudRepository<Game>
    {
        IEnumerable<Game> GetAll();
    }
}
