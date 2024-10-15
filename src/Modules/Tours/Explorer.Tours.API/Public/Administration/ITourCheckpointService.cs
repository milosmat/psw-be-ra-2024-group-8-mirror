using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Administration;
public interface ITourCheckpointService
{
    Result<PagedResult<TourCheckpointDto>> GetPaged(int page, int pageSize);
    Result<TourCheckpointDto> Create(TourCheckpointDto checkpoint);
    Result<TourCheckpointDto> Update(TourCheckpointDto checkpoint);
    Result Delete(int id);
}
