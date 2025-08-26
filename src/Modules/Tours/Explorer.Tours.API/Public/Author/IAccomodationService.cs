using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Author
{
    public interface IAccomodationService
    {
        Result<PagedResult<AccomodationDTO>> GetPaged(int page, int pageSize);
        Result<AccomodationDTO> Get(int id);
        Result<AccomodationDTO> Create(AccomodationDTO accomodationDto);
        Result<AccomodationDTO> Update(AccomodationDTO accomodationDto);
        Result Delete(int id);
    }
}
