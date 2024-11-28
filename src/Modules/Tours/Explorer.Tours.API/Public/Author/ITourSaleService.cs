using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Author;

public interface ITourSaleService
{
    Result<PagedResult<TourSaleDto>> GetPaged(int page, int pageSize);
    Result<TourSaleDto> Create(TourSaleDto tourSale);
    Result<TourSaleDto> Update(TourSaleDto tourSale);
    Result<TourSaleDto> Get(int id);
    Result Delete(int id);
}
