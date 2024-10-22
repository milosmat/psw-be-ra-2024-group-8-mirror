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
    public interface IObjectService
    {
        Result<PagedResult<ObjectDTO>> GetPaged(int page, int pageSize);
        Result<ObjectDTO> Get(int id);
        Result<ObjectDTO> Create(ObjectDTO objectDto);
        Result<ObjectDTO> Update(ObjectDTO objectDto);
        Result Delete(int id);
    }
}
