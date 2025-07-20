using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

public interface IUserRepository: ICrudRepository<User>
{
    bool Exists(string username);
    User? GetActiveByName(string username);
    User Create(User user);
    long GetPersonId(long userId);
    User GetUser(long userId);
    List<User> GetUsersByRole(UserRole role);
    User? GetByUsername(string username);
    void UpdateUserStatus(long id, bool isActive);

}