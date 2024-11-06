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
        private readonly ICrudRepository<VisitedCheckpoint> _visitedCheckpointRepository;
        private readonly ICrudRepository<TourCheckpoint> _tourCheckpointRepository;
        private readonly IMapper _mapper;

        public TourExecutionService(ICrudRepository<TourExecution> tourExecutionRepository,
                                    ICrudRepository<Tour> tourRepository,
                                    ICrudRepository<VisitedCheckpoint> visitedCheckpointRepository,
                                    ICrudRepository<TourCheckpoint> tourCheckpointRepository,
                                    IMapper mapper) : base(tourExecutionRepository, mapper)
        {
            _tourExecutionRepository = tourExecutionRepository;
            _tourRepository = tourRepository;
            _visitedCheckpointRepository = visitedCheckpointRepository;
            _tourCheckpointRepository = tourCheckpointRepository;
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

        public Result CheckForVisitedCheckpoints(int executionId, double lat, double lon)
        {
            const double maxDistanceMeters = 500;
            var tourExecution = _tourExecutionRepository.Get(executionId, te => te.visitedCheckpoints);
            if (tourExecution == null) return Result.Fail("Tour execution not found.");

            int tourId = tourExecution.TourId;

            var tour = _tourRepository.Get(tourId, t => t.TourCheckpoints);

            if (tour == null) return Result.Fail("Tour not found.");


            foreach (var ch in tour.TourCheckpoints)
            {
                bool alreadyVisited = tourExecution.visitedCheckpoints.Any(vc => vc.CheckpointId == ch.Id);
                if (!alreadyVisited)
                {
                    if (IsNearby(lat, lon, ch.Latitude, ch.Longitude, maxDistanceMeters))
                    { 
                        VisitCheckpoint(executionId, (int)ch.Id);
                        return Result.Ok();
                    }
                }
            }
            return Result.Fail("No nearby checkpoints -> 2");
        }

        private bool IsNearby(double lat1, double lon1, double lat2, double lon2, double maxDistanceMeters)
        {
            // Prag razlike u stepenima za otprilike 500 metara
            double degreeThreshold = 0.01;

            // Razlika u latitudi i longitudi
            double dLat = Math.Abs(lat1 - lat2);
            double dLon = Math.Abs(lon1 - lon2);

            // Proverava da li su obe razlike unutar praga
            return dLat <= degreeThreshold && dLon <= degreeThreshold;
        }


        public Result VisitCheckpoint(int executionId, int checkpointId)
        {
            var tourExecution = _tourExecutionRepository.Get(executionId);
            if (tourExecution == null) return Result.Fail("Tour execution not found.");

            var visitedCheckpoint = new VisitedCheckpoint(checkpointId, DateTime.UtcNow);
            tourExecution.VisitCheckpoint(visitedCheckpoint);
            _visitedCheckpointRepository.Create(visitedCheckpoint);
            return Result.Ok();
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
