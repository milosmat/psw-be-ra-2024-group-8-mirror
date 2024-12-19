using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Games.Core.Domain;
using Explorer.Games.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Explorer.Games.Infrastructure.Database.Repositories
{
    public class GamesDatabaseRepository : CrudDatabaseRepository<Game, GamesContext>, IGamesRepository
    {
        public GamesDatabaseRepository(GamesContext dbContext) : base(dbContext)
        {
        }

        public new Game? Get(long id)
        {
            return DbContext.Games
                .Include(g => g.Scores) // Uključivanje povezane kolekcije Scores
                .FirstOrDefault(g => g.Id == id);
        }

        public IEnumerable<Game> GetAll()
        {
            return DbContext.Games
                .Include(g => g.Scores) // Uključivanje povezane kolekcije Scores
                .ToList();
        }

        public new PagedResult<Game> GetPaged(int page, int pageSize)
        {
            var task = DbContext.Games
                .Include(g => g.Scores)
                .GetPagedById(page, pageSize);
            task.Wait();
            return task.Result;
        }

        public new Game Update(Game aggregateRoot)
        {
            DbContext.Entry(aggregateRoot).State = EntityState.Modified;

            // Ažuriraj Scores ručno ako je potrebno
            foreach (var score in aggregateRoot.Scores)
            {
                if (DbContext.Entry(score).State == EntityState.Detached)
                {
                    DbContext.Entry(score).State = EntityState.Added;
                }
            }

            DbContext.SaveChanges();
            return aggregateRoot;
        }
    }
}
