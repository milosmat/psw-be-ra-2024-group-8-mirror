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
    public interface ITourService
    {
        Result<PagedResult<TourDTO>> GetPaged(int page, int pageSize);
        Result<TourDTO> Get(int id);
        Result<TourDTO> Create(TourDTO tourDto);
        Result<TourDTO> Update(TourDTO tourDto);
        Result Delete(int id);

        Result<List<long>> GetEquipmentIds(int tourId);
        Result AddEquipmentId(int tourId, long equipmentId);
        Result RemoveEquipmentId(int tourId, long equipmentId);
    }
}
