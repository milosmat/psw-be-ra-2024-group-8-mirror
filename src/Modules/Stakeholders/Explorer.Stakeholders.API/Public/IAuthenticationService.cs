using Explorer.Stakeholders.API.Dtos;
using FluentResults;

namespace Explorer.Stakeholders.API.Public;

public interface IAuthenticationService
{
    Result<AuthenticationTokensDto> Login(CredentialsDto credentials);
    Task<Result> GenerateAndSendCouponToUserAsync(UserDto user);
    List<UserDto> GetAllTourists();

    long GetUserId(string username);
    Result RegisterUser(AccountRegistrationDto account);
    Result<AuthenticationTokensDto> Register(AccountRegistrationDto account);
    UserDto GetByUsername(string username);
    void UpdateUserStatus(long id, bool isActive);
}