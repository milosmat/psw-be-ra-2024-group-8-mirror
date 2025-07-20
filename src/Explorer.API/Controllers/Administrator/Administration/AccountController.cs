using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Administration;
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

        public AccountController(IAdministratorService administratorService)
        {
            _administratorService = administratorService;
        }

        [HttpGet]
        public ActionResult<PagedResult<AccountInformationDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _administratorService.GetPaged(page, pageSize);
            if (result.IsFailed)
            {
                return StatusCode(500, "Unexpected error occurred while collecting accounts data.");
            }

            return Ok(result.Value);
        }

        [HttpPut("{id:int}")]
        public ActionResult<AccountInformationDto> Update([FromBody] AccountInformationDto info)
        {

            var result = _administratorService.UpdateUserStatus(info.Id, info.IsActive);

            if(result.IsFailed)
            {
                return BadRequest(result.Errors.Last());
            }

            if(info.IsActive)
            {
                return Ok("Account is unblocked.");
            }
            return Ok("Account is blocked");
            
        }

    }
}
