using Microsoft.Extensions.DependencyInjection;
using Explorer.Games.Core.Mappers;
using Explorer.Games.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Games.API.Public.Tourist;
using Explorer.Games.Core.UseCases.Tourist;
using Explorer.Games.Core.Domain.RepositoryInterfaces;
using Explorer.Games.Infrastructure.Database.Repositories;

namespace Explorer.Games.Infrastructure
{
    public static class GamesStartup
    {
        public static IServiceCollection ConfigureGamesModule(this IServiceCollection services)
        {
            // Registracija AutoMapper profila
            services.AddAutoMapper(typeof(GamesProfile).Assembly);

            SetupCore(services);
            SetupInfrastructure(services);
            return services;
        }

        private static void SetupInfrastructure(IServiceCollection services)
        {
            services.AddScoped<IGamesRepository, GamesDatabaseRepository>();

            // Registracija DbContext-a za GamesContext
            services.AddDbContext<GamesContext>(opt =>
                opt.UseNpgsql(DbConnectionStringBuilder.Build("games"),
                    x => x.MigrationsHistoryTable("__EFMigrationsHistory", "games")));
        }

        private static void SetupCore(IServiceCollection services)
        {
            // Dodajte registracije Core servisa ovde (ako su potrebni)
            services.AddScoped<IGameService, GameService>();
        }
    }
}
