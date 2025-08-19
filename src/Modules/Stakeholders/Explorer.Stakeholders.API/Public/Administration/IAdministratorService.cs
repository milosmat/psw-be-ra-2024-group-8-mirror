using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public.Administration
{
    public interface IAdministratorService
    {
        Result<PagedResult<AccountInformationDto>> GetPaged(int page, int pageSize);
        //Result<AccountInformationDto> Update(AccountInformationDto accountInformation);

        Result UpdateUserStatus(long userId, bool isActive);
    }

}
