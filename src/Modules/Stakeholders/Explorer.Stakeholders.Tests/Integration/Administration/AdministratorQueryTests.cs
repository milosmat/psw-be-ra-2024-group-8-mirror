using Explorer.API.Controllers.Administrator.Administration;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.API.Public.Administration;

namespace Explorer.Stakeholders.Tests.Integration.Administration
{
    [Collection("Sequential")]
    public class AdministratorQueryTests : BaseStakeholdersIntegrationTest
    {
        public AdministratorQueryTests(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void Retrieves_all()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as PagedResult<AccountInformationDto>;

            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(7);
            result.TotalCount.ShouldBe(7);
        }

        private static AccountController CreateController(IServiceScope scope)
        {
            return new AccountController(scope.ServiceProvider.GetRequiredService<IAdministratorService>(),
                                         scope.ServiceProvider.GetRequiredService<ICrudRepository<Person>>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
