using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.API.Public.Author;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Mappers;
using Explorer.Tours.Core.UseCases.Administration;
using Explorer.Tours.Core.UseCases.Author;
using Explorer.Tours.Core.UseCases.Tourist;
using Explorer.Tours.Infrastructure.Database;
using Explorer.Tours.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Object = Explorer.Tours.Core.Domain.Object;

namespace Explorer.Tours.Infrastructure;

public static class ToursStartup
{
    public static IServiceCollection ConfigureToursModule(this IServiceCollection services)
    {
        // Registers all profiles since it works on the assembly
        services.AddAutoMapper(typeof(ToursProfile).Assembly);
        SetupCore(services);
        SetupInfrastructure(services);
        return services;
    }
    
    private static void SetupCore(IServiceCollection services)
    {
        services.AddScoped<IEquipmentService, EquipmentService>();
        services.AddScoped<ITourPreferenceService, TourPreferencesService>();
        services.AddScoped<ITourService, TourService>();

        services.AddScoped<ITourReviewService, TourReviewService>();

        services.AddScoped<ITourCheckpointService, TourCheckpointService>();

        services.AddScoped<ITouristEquipmentService, TouristEquipmentService>();

        services.AddScoped<IObjectService, ObjectService>();
        services.AddScoped<ITouristPositionService, TouristPositionService>();
        services.AddScoped<ITourExecutionService, TourExecutionService>();

    }

    private static void SetupInfrastructure(IServiceCollection services)
    {
        services.AddScoped(typeof(ICrudRepository<Equipment>), typeof(CrudDatabaseRepository<Equipment, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<TourPreferences>), typeof(CrudDatabaseRepository<TourPreferences, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<Tour>), typeof(CrudDatabaseRepository<Tour, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<TourReview>), typeof(CrudDatabaseRepository<TourReview, ToursContext>));

        services.AddScoped(typeof(ICrudRepository<TourCheckpoint>), typeof(CrudDatabaseRepository<TourCheckpoint, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<TourExecution>), typeof(CrudDatabaseRepository<TourExecution, ToursContext>));
        services.AddScoped<ITouristEquipmentRepository, TouristEquipmentDatabaseRepository>();

        services.AddScoped(typeof(ICrudRepository<Object>), typeof(CrudDatabaseRepository<Object, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<TouristPosition>), typeof(CrudDatabaseRepository<TouristPosition, ToursContext>));
        services.AddScoped(typeof(ICrudRepository<VisitedCheckpoint>), typeof(CrudDatabaseRepository<VisitedCheckpoint, ToursContext>));

        services.AddScoped(typeof(ITourRepository), typeof(ToursDatabaseRepository));
        services.AddDbContext<ToursContext>(opt =>
            opt.UseNpgsql(DbConnectionStringBuilder.Build("tours"),
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "tours")));
    }
}