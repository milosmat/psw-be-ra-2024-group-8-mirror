using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Payments.Infrastructure.Database.Repositories;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.API.Public.Administration;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.Clubs;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Mappers;
using Explorer.Stakeholders.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Stakeholders.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection.Metadata;
using System.Xml.Linq;
using Explorer.Stakeholders.Core.Domain.Clubs;
using Explorer.Stakeholders.Core.Security;

namespace Explorer.Stakeholders.Infrastructure;

public static class StakeholdersStartup
{
    public static IServiceCollection ConfigureStakeholdersModule(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(StakeholderProfile).Assembly);
        SetupCore(services);
        SetupInfrastructure(services);
        return services;
    }
    
    private static void SetupCore(IServiceCollection services)
    {
        services.AddScoped<IAdministratorService, AdministratorService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ITokenGenerator, JwtGenerator>();
        services.AddScoped<IEditAccountService, EditAccountService>();
        services.AddScoped<IClubRepository, ClubDatabaseRepository>();
        services.AddScoped<IClubService, ClubService>();
        services.AddScoped<IMembershipRequestService, MembershipRequestService>();
        services.AddScoped<IAppRatingService, AppRatingService>();
        services.AddScoped<IProblemService, ProblemService>();
        services.AddScoped<ITourProblemService, TourProblemService>();
        services.AddScoped<IFollowersService, FollowersService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddHttpClient<IExchangeRateService, ExchangeRateService>();


    }

    private static void SetupInfrastructure(IServiceCollection services)
    {
        services.AddScoped(typeof(ICrudRepository<User>), typeof(CrudDatabaseRepository<User, StakeholdersContext>));
        services.AddScoped(typeof(ICrudRepository<Problem>), typeof(CrudDatabaseRepository<Problem, StakeholdersContext>));
        services.AddScoped<IUserRepository, UserDatabaseRepository>();
        services.AddScoped<IPersonRepository, PersonDatabaseRepository>();
        services.AddScoped<ITourProblemRepository, TourProblemDatabaseRepository>();
        services.AddScoped(typeof(ICrudRepository<Account>),typeof(CrudDatabaseRepository<Account,StakeholdersContext>));

        services.AddScoped(typeof(ICrudRepository<Club>), typeof(CrudDatabaseRepository<Club, StakeholdersContext>));
        services.AddScoped(typeof(ICrudRepository<MembershipRequest>), typeof(CrudDatabaseRepository<MembershipRequest, StakeholdersContext>));

        services.AddScoped(typeof(ICrudRepository<AppRating>), typeof(CrudDatabaseRepository<AppRating, StakeholdersContext>));

        services.AddScoped(typeof(ICrudRepository<Followers>), typeof(CrudDatabaseRepository<Followers, StakeholdersContext>));
        services.AddScoped(typeof(ICrudRepository<Message>), typeof(CrudDatabaseRepository<Message, StakeholdersContext>));

        services.AddScoped(typeof(ICrudRepository<Notification>), typeof(CrudDatabaseRepository<Notification, StakeholdersContext>));

        services.AddScoped<INotificationRepository, NotificationRepository>();
        
        services.AddScoped<IMessageRepository, MessageRepository>();
        
        services.AddScoped(typeof(ICrudRepository<Wallet>), typeof(CrudDatabaseRepository<Wallet, StakeholdersContext>));

        services.AddScoped<IWalletRepository, WalletDataBaseRepository>();
        
        services.AddDbContext<StakeholdersContext>(opt =>
            opt.UseNpgsql(DbConnectionStringBuilder.Build("stakeholders"),
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "stakeholders")));
    }

}

   

