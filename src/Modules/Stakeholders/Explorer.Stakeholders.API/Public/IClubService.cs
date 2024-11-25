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
    ClubDto Get(int id);
    ClubDto Create(ClubDto clubDto);
    ClubDto Update(ClubDto clubDto);
    void Delete(int id);
    PagedResult<ClubDto> GetPaged(int page, int pageSize);

}
