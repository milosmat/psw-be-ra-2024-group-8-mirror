using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public
{
    public interface IAppRatingService
    {
        Result<AppRatingDto> Create(AppRatingDto appRatingInfo);
        Result<PagedResult<AppRatingDto>> GetPaged(int page, int pageSize);
        Result Delete(int id);
    }
}
