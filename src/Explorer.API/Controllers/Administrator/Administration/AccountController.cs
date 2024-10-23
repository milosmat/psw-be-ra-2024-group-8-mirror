using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Administration;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator.Administration
{
    [Authorize(Policy = "administratorPolicy")]
    [Route("api/administration/accounts")]
    public class AccountController : BaseApiController
    {
        private readonly IAdministratorService _administratorService;
        private readonly ICrudRepository<Person> _personRepository;

        public AccountController(IAdministratorService administratorService, ICrudRepository<Person> personRepository)
        {
            _administratorService = administratorService;
            _personRepository = personRepository;
        }

        [HttpGet]
        public ActionResult<PagedResult<AccountInformationDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _administratorService.GetPaged(page, pageSize);
            for(int i = 0; i < result.Value.Results.Count; i++)
            {
                try
                {
                    var person = _personRepository.Get(result.Value.Results[i].Id);
                    result.Value.Results[i].Email = person.Email; 
                }
                catch (KeyNotFoundException)
                {
                    result.Value.Results[i].Email = "None";
                }

            }
            return CreateResponse(result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<AccountInformationDto> Update([FromBody] AccountInformationDto newInfo)
        {
            var result = _administratorService.Update(newInfo);
            return CreateResponse(result);
        }
      
    }
}
