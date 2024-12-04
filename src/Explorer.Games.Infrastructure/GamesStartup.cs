using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Games.Core.Mappers;

namespace Explorer.Games.Infrastructure
{

    public static class GamesStartup
    {
        public static IServiceCollection ConfigureGamesModule(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(GamesProfile).Assembly); // Preduslov da imamo ovu liniju koda je da smo definisali već Profile klasu u Core/Mappers
            SetupCore(services);
            SetupInfrastructure(services);
            return services;
        }

        private static void SetupInfrastructure(IServiceCollection services)
        {
            
        }

        private static void SetupCore(IServiceCollection services)
        {
            
        }
    }
}
