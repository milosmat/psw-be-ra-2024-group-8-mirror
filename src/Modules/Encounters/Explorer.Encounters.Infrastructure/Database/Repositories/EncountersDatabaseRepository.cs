using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Explorer.Encounters.Infrastructure.Database.Repositories
{
    public class EncountersDatabaseRepository : CrudDatabaseRepository<Encounter, EncountersContext>, IEncounterRepository
    {
        public EncountersDatabaseRepository(EncountersContext dbContext) : base(dbContext)
        {
        }

        public new Encounter? Get(long id)
        {
            return DbContext.Encounters
                .Where(e => e.Id == id)
                .FirstOrDefault(); // Include relationships if necessary
        }

        public IEnumerable<Encounter> GetAll()
        {
            return DbContext.Encounters.ToList(); // Dohvata sve izazove iz baze
        }

        public new PagedResult<Encounter> GetPaged(int page, int pageSize)
        {
            var task = DbContext.Encounters.GetPagedById(page, pageSize);
            task.Wait();
            return task.Result;
        }

        public new Encounter Update(Encounter aggregateRoot)
        {
            DbContext.Entry(aggregateRoot).State = EntityState.Modified;
            DbContext.SaveChanges();
            return aggregateRoot;
        }
    }
}
