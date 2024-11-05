using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.Domain;
using FluentResults;
using System;

namespace Explorer.Tours.Core.UseCases.Tourist
{
    public class TourExecutionService : CrudService<TourExecutionDto, TourExecution>, ITourExecutionService
    {
        private readonly ICrudRepository<TourExecution> _tourExecutionRepository;
        private readonly ICrudRepository<Tour> _tourRepository;
        private readonly IMapper _mapper;

        public TourExecutionService(ICrudRepository<TourExecution> tourExecutionRepository,
                                    ICrudRepository<Tour> tourRepository,
                                    IMapper mapper) : base(tourExecutionRepository, mapper)
        {
            _tourExecutionRepository = tourExecutionRepository;
            _tourRepository = tourRepository;
            _mapper = mapper;
        }

        public Result<TourExecutionDto> StartTourExecution(int tourId, int userId)
        {
            // Pribavi turu
            var tour = _tourRepository.Get(tourId);
            if (tour == null) return Result.Fail("Tour not found.");

            // Proverava da li je tura dostupna za pokretanje
            if (tour.Status is not (Domain.TourStatus)TourStatus.PUBLISHED and not (Domain.TourStatus)TourStatus.ARCHIVED)
                return Result.Fail("Tour is not available for starting.");

            // Kreira novu sesiju ture koristeći `tourId`, `userId`, i početnu lokaciju
            var tourExecution = new TourExecution(tourId, userId); // beleži početno vreme i status
            var createdTourExecution = _tourExecutionRepository.Create(tourExecution);

            // Mapira `TourExecution` na `TourExecutionDto` i vraća rezultat
            var executionDto = _mapper.Map<TourExecutionDto>(createdTourExecution);
            return Result.Ok(executionDto);
        }

        public Result CompleteTourExecution(int executionId)
        {
            var tourExecution = _tourExecutionRepository.Get(executionId);
            if (tourExecution == null) return Result.Fail("Tour execution not found.");

            tourExecution.CompleteTour();
            _tourExecutionRepository.Update(tourExecution);

            return Result.Ok();
        }

        public Result AbandonTourExecution(int executionId)
        {
            var tourExecution = _tourExecutionRepository.Get(executionId);
            if (tourExecution == null) return Result.Fail("Tour execution not found.");

            tourExecution.AbandonTour();
            _tourExecutionRepository.Update(tourExecution);

            return Result.Ok();
        }
    }
}
