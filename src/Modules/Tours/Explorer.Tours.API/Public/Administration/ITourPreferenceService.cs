using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Tours.API.Public.Administration;

public interface ITourPreferenceService
{
    Result<PagedResult<TourPreferencesDto>> GetPaged(int page, int pageSize);
    Result<TourPreferencesDto> Create(TourPreferencesDto tourPreferences);
    Result<TourPreferencesDto> Update(TourPreferencesDto tourPreferences);
    Result Delete(int id);

}
