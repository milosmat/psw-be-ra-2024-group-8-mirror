using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Security;
using FluentResults;
using System.Net.Mail;

namespace Explorer.Stakeholders.Core.UseCases;

public class AuthenticationService : IAuthenticationService
{
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IPersonRepository _personRepository;
    private readonly ICouponSenderService _couponSenderService;
    private readonly IEmailService _emailService;
    private readonly IPasswordHasher _passwordHasher;
    private IMapper _mapper { get; set; }

    public AuthenticationService(IUserRepository userRepository, IPersonRepository personRepository,
        ITokenGenerator tokenGenerator,
        ICouponSenderService couponSenderService,
        IEmailService emailService,
        IPasswordHasher passwordHasher,
        IMapper mapper)
    {
        _tokenGenerator = tokenGenerator;
        _userRepository = userRepository;
        _personRepository = personRepository;
        _couponSenderService = couponSenderService;
        _emailService = emailService;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public Result<AuthenticationTokensDto> Login(CredentialsDto credentials)
    {
        var user = _userRepository.GetActiveByName(credentials.Username);
        if(user == null)
        {
            return Result.Fail(FailureCode.NotFound).WithError("User not found.");
        }
        bool isPasswordValid = _passwordHasher.VerifyPassword(credentials.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            return Result.Fail(FailureCode.BadRequest).WithError("Invalid password.");
        }

        return _tokenGenerator.GenerateAccessToken(user, user.Id);
    }

    public long GetUserId(string username)
    {

        var user = _userRepository.GetActiveByName(username);

        return user.Id;

    }

    public async Task<Result> GenerateAndSendCouponToUserAsync(UserDto user)
    {
        try
        {
            var person = _personRepository.GetByUserId(user.Id);
            var result = _couponSenderService.GenerateCouponToNewUser(user.Id);

            if (result.IsFailed)
            {
                return Result.Fail(FailureCode.CouponGenerationFailed);
            }
            string subject = "Welcome to GoTravel!";
            var body = $@"
                        <!DOCTYPE html>
                        <html>
                        <head>
                            <meta charset='utf-8'>
                            <title>Account Verification</title>
                        </head>
                        <body>
                           <p>Dear {person.Name},</p>
                           <p>Congratulations! Your registration on GoTravel has been successfully completed.</p>
                           <p>As a welcome gift, you have received a coupon with the code: {result.Value};</p>
                           <p>You can use this coupon to get a 5% discount on our platform. The coupon is valid for the next 30 days, so make sure to use it before it expires!</p>
                           <p>Best regards,</p>
                           <p>The Group-8 Team</p>
                        </body>
                        </html>
                    ";

            var emailSender = await _emailService.SendEmailToUserAsync(person.Email, subject, body);

            return Result.Ok();
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
    }

    public List<UserDto> GetAllTourists()
    {
        var tourists = _userRepository.GetUsersByRole(UserRole.Tourist);

        // Mapiramo User objekte u UserDto objekte
        return tourists.Select(user => new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Role = (int)user.Role,
            IsActive = user.IsActive
        }).ToList();
    }

    public Result<AuthenticationTokensDto> Register(AccountRegistrationDto account)
    {
        if (_userRepository.Exists(account.Username)) return Result.Fail(FailureCode.NonUniqueUsername);

        try
        {
            ValidateUserData(account.Name, account.Surname, account.Email);

            var passwordHash = _passwordHasher.HashPassword(account.Password);

            var user = _userRepository.Create(new User(account.Username, passwordHash, UserRole.Tourist, false));

            var person = _personRepository.Create(new Person(user.Id, account.Name, account.Surname, account.Email));

            return _tokenGenerator.GenerateAccessToken(user, person.Id);
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
    }

    public Result RegisterUser(AccountRegistrationDto account)
    {
        if (_userRepository.Exists(account.Username)) return Result.Fail(FailureCode.NonUniqueUsername);

        try
        {
            ValidateUserData(account.Name, account.Surname, account.Email);

            var passwordHash = _passwordHasher.HashPassword(account.Password);

            var user = _userRepository.Create(new User(account.Username, passwordHash, UserRole.Tourist, false));

            var person = _personRepository.Create(new Person(user.Id, account.Name, account.Surname, account.Email));

            return Result.Ok();
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
    }
    private void ValidateUserData(string name, string surname, string email)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.");
        if (string.IsNullOrWhiteSpace(surname)) throw new ArgumentException("Surname is required.");
        if (!MailAddress.TryCreate(email, out _)) throw new ArgumentException("Invalid email format.");
    }

    public UserDto GetByUsername(string username)
    {
        return _mapper.Map<UserDto>(_userRepository.GetByUsername(username));
    }
    public void UpdateUserStatus(long userId, bool isActive)
    {
        try
        {
            _userRepository.UpdateUserStatus(userId, true);
        }
        catch(KeyNotFoundException ex)
        {
            throw new KeyNotFoundException(ex.Message);
        }
    }
}