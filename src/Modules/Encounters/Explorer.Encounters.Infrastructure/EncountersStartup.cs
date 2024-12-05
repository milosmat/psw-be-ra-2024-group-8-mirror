using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Encounters.API.Public.Administrator;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using Explorer.Encounters.Core.Mappers;
using Explorer.Encounters.Core.UseCases.Administrator;
using Explorer.Encounters.Infrastructure.Database;
using Explorer.Encounters.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Explorer.Encounters.API.Public.Tourist; // Dodato za ITouristProfileService
using Explorer.Encounters.Core.UseCases.Tourist; // Dodato za TouristProfileService

namespace Explorer.Encounters.Infrastructure
{
    public static class EncountersStartup
    {
        public static IServiceCollection ConfigureEncountersModule(this IServiceCollection services)
        {
            // Registers AutoMapper profiles from the Encounters assembly
            services.AddAutoMapper(typeof(EncountersProfile).Assembly);

            SetupCore(services);
            SetupInfrastructure(services);

            return services;
        }

        private static void SetupCore(IServiceCollection services)
        {
            // Register core services
            services.AddScoped<IEncounterService, EncounterService>();
            services.AddScoped<ITouristProfileService, TouristProfileService>();
        }

        private static void SetupInfrastructure(IServiceCollection services)
        {
            // Register repositories
            services.AddScoped(typeof(ICrudRepository<Encounter>), typeof(CrudDatabaseRepository<Encounter, EncountersContext>));
            services.AddScoped<IEncounterRepository, EncountersDatabaseRepository>();
            services.AddScoped<ITouristProfileRepository, TouristProfileDatabaseRepository>();

            // Register DbContext
            services.AddDbContext<EncountersContext>(opt =>
                opt.UseNpgsql(DbConnectionStringBuilder.Build("encounters"),
                    x => x.MigrationsHistoryTable("__EFMigrationsHistory", "encounters")));
        }
    }
}
