using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Tours.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace Explorer.API.Controllers.Tourist;

[ApiController]
[Route("api/[controller]")]

public class EditAccountController : BaseApiController
{
    private readonly IEditAccountService _editAccountService;

    public EditAccountController(IEditAccountService editAccountService)
    {
        _editAccountService = editAccountService;
    }

    [HttpGet]
    public ActionResult<PagedResult<AccountDto>> Get([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _editAccountService.GetPaged(page, pageSize);
        return CreateResponse(result);
    }

    [HttpPut("{id:int}")]
    public ActionResult<AccountDto> Update([FromBody] AccountDto account)
    {
        var result = _editAccountService.Update(account);
        return CreateResponse(result);
    }

}
