using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Games.Core.Domain;
using Explorer.Games.Core.Domain.RepositoryInterfaces;
using Explorer.Games.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Explorer.Games.Infrastructure.Database.Repositories
{
    public class GameScoreDatabaseRepository : CrudDatabaseRepository<GameScore, GamesContext>, IGameScoreRepository
    {
        public GameScoreDatabaseRepository(GamesContext dbContext) : base(dbContext)
        {
        }

        public new GameScore? Get(long id)
        {
            return DbContext.GameScores
                .FirstOrDefault(gs => gs.Id == id);
        }

        public IEnumerable<GameScore> GetAll()
        {
            return DbContext.GameScores
                .ToList(); // Remove invalid Include calls
        }

        public new PagedResult<GameScore> GetPaged(int page, int pageSize)
        {
            var task = DbContext.GameScores
                .OrderBy(gs => gs.Id) // Assuming paging is based on Id
                .GetPagedById(page, pageSize);
            task.Wait();
            return task.Result;
        }

        public new GameScore Update(GameScore entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;

            DbContext.SaveChanges();
            return entity;
        }

        public new GameScore Create(GameScore entity)
        {
            DbContext.GameScores.Add(entity);
            DbContext.SaveChanges();
            return entity;
        }
    }
}