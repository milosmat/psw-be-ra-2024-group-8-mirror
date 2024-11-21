using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
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
        services.AddScoped<IClubService, ClubService>();
        services.AddScoped<IMembershipRequestService, MembershipRequestService>();
        services.AddScoped<IClubRepository, ClubDatabaseRepository>();
        services.AddScoped<IAppRatingService, AppRatingService>();
        services.AddScoped<IProblemService, ProblemService>();
        services.AddScoped<ITourProblemService, TourProblemService>();
        services.AddScoped<IFollowersService, FollowersService>();
        services.AddScoped<INotificationService, NotificationService>();
    }

    private static void SetupInfrastructure(IServiceCollection services)
    {
        services.AddScoped(typeof(ICrudRepository<User>), typeof(CrudDatabaseRepository<User, StakeholdersContext>));
        services.AddScoped(typeof(ICrudRepository<Person>), typeof(CrudDatabaseRepository<Person, StakeholdersContext>));
        services.AddScoped(typeof(ICrudRepository<Problem>), typeof(CrudDatabaseRepository<Problem, StakeholdersContext>));
        services.AddScoped<IUserRepository, UserDatabaseRepository>();
        services.AddScoped<ITourProblemRepository, TourProblemDatabaseRepository>();
        services.AddScoped(typeof(ICrudRepository<Account>),typeof(CrudDatabaseRepository<Account,StakeholdersContext>));

        services.AddScoped(typeof(ICrudRepository<Club>), typeof(CrudDatabaseRepository<Club, StakeholdersContext>));
        services.AddScoped(typeof(ICrudRepository<MembershipRequest>), typeof(CrudDatabaseRepository<MembershipRequest, StakeholdersContext>));

        services.AddScoped(typeof(ICrudRepository<AppRating>), typeof(CrudDatabaseRepository<AppRating, StakeholdersContext>));

        services.AddScoped(typeof(ICrudRepository<Followers>), typeof(CrudDatabaseRepository<Followers, StakeholdersContext>));

        services.AddScoped(typeof(ICrudRepository<Notification>), typeof(CrudDatabaseRepository<Notification, StakeholdersContext>));

        services.AddScoped<INotificationRepository, NotificationRepository>();
        
        services.AddScoped<IMessageRepository, MessageRepository>();

        services.AddDbContext<StakeholdersContext>(opt =>
            opt.UseNpgsql(DbConnectionStringBuilder.Build("stakeholders"),
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "stakeholders")));
    }

}

   

