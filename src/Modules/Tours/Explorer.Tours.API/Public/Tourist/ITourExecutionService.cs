using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Tourist
{
    public interface ITourExecutionService
    {
        Result<TourExecutionDto> StartTourExecution(int tourId, int userId);//, LocationDto startLocation);
        Result CompleteTourExecution(int executionId);
        Result AbandonTourExecution(int executionId);
        Result<List<TourDTO>> GetAllTours();
        Result<TourExecutionDto> GetTourExecutionStatus(int tourId, int userId);
    }
}
