using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain.Clubs;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class ClubDatabaseRepository : CrudDatabaseRepository<Club, StakeholdersContext>, IClubRepository
    {
        public ClubDatabaseRepository(StakeholdersContext dbContex) : base(dbContex) { }

        public new Club? Get(int id)
        {
            return DbContext.Clubs.Where(t => t.Id == id)
            .Include(t => t.Requests!).FirstOrDefault();
        }

        public Club Update(Club aggregateRoot)
        {
            DbContext.Entry(aggregateRoot).State = EntityState.Modified;
            DbContext.Clubs.Update(aggregateRoot);
            DbContext.SaveChanges();
            return aggregateRoot;
        }
    }
}
