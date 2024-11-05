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
        private readonly IMapper _mapper;

        public TourExecutionService(ICrudRepository<TourExecution> tourExecutionRepository,
                                    ICrudRepository<Tour> tourRepository,
                                    ICrudRepository<VisitedCheckpoint> visitedCheckpointRepository,
                                    IMapper mapper) : base(tourExecutionRepository, mapper)
        {
            _tourExecutionRepository = tourExecutionRepository;
            _tourRepository = tourRepository;
            _visitedCheckpointRepository = visitedCheckpointRepository;
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
            var tourExecution = _tourExecutionRepository.Get(executionId);
            if (tourExecution == null) return Result.Fail("Tour execution not found.");

            int tourId = tourExecution.TourId;

            var tour = _tourRepository.Get(tourId);
            if (tour == null) return Result.Fail("Tour not found.");

            foreach(var ch in tour.TourCheckpoints)
            {
                bool alreadyVisited = tourExecution.visitedCheckpoints.Any(vc => vc.CheckpointId == ch.Id);
                if (!alreadyVisited)
                {
                    VisitCheckpoint(executionId, (int)ch.Id);
                    return Result.Ok();
                }
                else
                {
                    return Result.Fail("Checkpoint already visited.");
                }
            }
            return Result.Fail("No nearby checkpoints");
        }

        private bool IsNearby(double lat1, double lon1, double lat2, double lon2, double maxDistanceMeters)
        {
            const double EarthRadiusMeters = 6371000; // Poluprečnik Zemlje u metrima

            // Konverzija stepena u radijane
            double dLat = DegreesToRadians(lat2 - lat1);
            double dLon = DegreesToRadians(lon2 - lon1);

            // Haversinova formula
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Udaljenost između tačaka u metrima
            double distance = EarthRadiusMeters * c;

            return distance <= maxDistanceMeters;
        }

        // Pomoćna metoda za konverziju stepena u radijane
        private double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public Result VisitCheckpoint(int executionId, int checkpointId)
        {
            var tourExecution = _tourExecutionRepository.Get(executionId);
            if (tourExecution == null) return Result.Fail("Tour execution not found.");

            var visitedCheckpoint = new VisitedCheckpoint(checkpointId, DateTime.UtcNow);
            tourExecution.VisitCheckpoint(visitedCheckpoint);
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
