using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class PersonDatabaseRepository : CrudDatabaseRepository<Person, StakeholdersContext>, IPersonRepository
    {
        public PersonDatabaseRepository(StakeholdersContext context) : base(context) { }

        public Person? GetByUserId(long userId)
        {
            return DbContext.People.FirstOrDefault(p => p.UserId == userId);
        }

        public List<Person> GetByUserIds(List<long> userIds)
        {
            return DbContext.People.Where(p => userIds.Contains(p.UserId)).ToList();        
        }

    }
}
