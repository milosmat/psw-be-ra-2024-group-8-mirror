using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Tourist;

public interface ITouristEquipmentService
{
    Result<PagedResult<TouristEquipmentDTO>> GetPaged(int page, int pageSize);
    Result<TouristEquipmentDTO> Create(TouristEquipmentDTO touristEquipment);
    Result<TouristEquipmentDTO> Get(int id);
    Result Delete(int id);

    Result<TouristEquipmentDTO> FindByTouristAndEquipment(long touristId, long equipmentId);
}
