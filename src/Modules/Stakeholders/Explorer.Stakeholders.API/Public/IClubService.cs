using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public;

public interface IClubService
{
    Result<ClubDto> Get(int id);
    Result<ClubDto> Create(ClubDto clubDto);
    Result<ClubDto> Update(ClubDto clubDto);
    Result Delete(int id);
    Result<PagedResult<ClubDto>> GetPaged(int page, int pageSize);

}
