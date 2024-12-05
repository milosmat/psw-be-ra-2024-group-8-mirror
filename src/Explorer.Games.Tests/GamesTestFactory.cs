using Explorer.BuildingBlocks.Tests;
using Explorer.Games.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Games.Tests
{
    public class GamesTestFactory : BaseTestFactory<GamesContext>
    {
        public GamesTestFactory() { }
        protected override IServiceCollection ReplaceNeededDbContexts(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<GamesContext>));
            services.Remove(descriptor!);
            services.AddDbContext<GamesContext>(SetupTestContext());

            /*
            descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<OTHER_MODULE_NAMEContext>));
            services.Remove(descriptor!);
            services.AddDbContext<OTHER_MODULE_NAMEContext>(SetupTestContext());
            */

            return services;
        }
    }
}
