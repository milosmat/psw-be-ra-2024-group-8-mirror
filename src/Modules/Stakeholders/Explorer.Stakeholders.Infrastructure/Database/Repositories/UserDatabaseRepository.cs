using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories;

public class UserDatabaseRepository : CrudDatabaseRepository<User, StakeholdersContext> , IUserRepository
{
    public UserDatabaseRepository(StakeholdersContext dbContext) : base(dbContext) { }

    public bool Exists(string username)
    {
        return DbContext.Users.Any(user => user.Username == username);
    }

    public User? GetActiveByName(string username)
    {
        return DbContext.Users.FirstOrDefault(user => user.Username == username && user.IsActive);
    }

    public long GetPersonId(long userId)
    {
        var person = DbContext.People.FirstOrDefault(i => i.UserId == userId);
        if (person == null) throw new KeyNotFoundException("Not found.");
        return person.Id;
    }

    public User GetUser(long userId)
    {
        return DbContext.Users.FirstOrDefault(i => i.Id == userId);
    }

    public List<User> GetUsersByRole(UserRole role)
    {
        return DbContext.Users.Where(u => u.Role == role).ToList();
    }
    public User? GetByUsername(string username)
    {
        return DbContext.Users.AsNoTracking().FirstOrDefault(u => u.Username.Equals(username));
    }
   
    public void UpdateUserStatus(long userId, bool isActive)
    {
        var user = DbContext.Users.Find(userId);

        if (user == null) throw new KeyNotFoundException();

        user.IsActive = isActive;
        DbContext.SaveChanges();
    }
}