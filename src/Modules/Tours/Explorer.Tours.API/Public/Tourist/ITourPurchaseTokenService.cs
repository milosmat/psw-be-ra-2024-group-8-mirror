using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;

namespace Explorer.Tours.API.Public.Tourist
{
    public interface ITourPurchaseTokenService
    {
        Result<TourPurchaseTokenDTO> Get(int id);
        Result<TourPurchaseTokenDTO> Create(TourPurchaseTokenDTO tokenDto);
        Result<TourPurchaseTokenDTO> Update(TourPurchaseTokenDTO tokenDto);
        Result Delete(int id);
        Result<PagedResult<TourPurchaseTokenDTO>> GetPaged(int page, int pageSize);
        Result<List<TourDTO>> GetPurchasedToursByTouristId(long touristId);
    }
}
