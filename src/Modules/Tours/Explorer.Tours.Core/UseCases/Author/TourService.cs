using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Author;
using Explorer.Tours.Core.Domain;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.Author
{
    public class TourService : CrudService<TourDTO, Tour>, ITourService
    {
        private readonly ICrudRepository<TourCheckpoint> _tourCheckpointRepository;
        private readonly ICrudRepository<Equipment> _equipmentRepository;
        public TourService(ICrudRepository<Tour> repository, IMapper mapper, 
            ICrudRepository<TourCheckpoint> tourCheckpointRepository,
            ICrudRepository<Equipment> equipmentRepository) : base(repository, mapper) 
        {
            _tourCheckpointRepository = tourCheckpointRepository;
            _equipmentRepository = equipmentRepository;
        }
        public Result<List<long>> GetCheckpointIds(int tourId)
        {
            try
            {
                var tourResult = CrudRepository.Get(tourId);
                List<long> result = new List<long>();
                foreach (var item in tourResult.TourCheckpoints)
                {
                    result.Add(item.Id);
                }
                return Result.Ok(result);
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }
        public Result<List<long>> GetEquipmentIds(int tourId)
        {
            try
            {
                var tourResult = CrudRepository.Get(tourId);
                List<long> result = new List<long>();
                foreach (var item in tourResult.Equipments)
                {
                    result.Add(item.Id);
                }
                return Result.Ok(result);
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }

        public Result AddCheckpointId(int tourId, long checkpointId)
        {
            try
            {
                var tour = CrudRepository.Get(tourId);
                var tourCheckpoint = _tourCheckpointRepository.Get(checkpointId);

                if (!tour.TourCheckpoints.Contains(tourCheckpoint))
                {
                    tour.TourCheckpoints.Add(tourCheckpoint);
                    CrudRepository.Update(tour);
                    return Result.Ok();
                }
                return Result.Fail(FailureCode.InvalidArgument).WithError("Checkpoint ID already exists in the list");
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }
        public Result AddEquipmentId(int tourId, long equipmentId)
        {
            try
            {
                var tour = CrudRepository.Get(tourId);
                var equipment = _equipmentRepository.Get(equipmentId);

                if (!tour.Equipments.Contains(equipment))
                {
                    tour.Equipments.Add(equipment);
                    CrudRepository.Update(tour); 
                    return Result.Ok();
                }
                return Result.Fail(FailureCode.InvalidArgument).WithError("Equipment ID already exists in the list");
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }
        public Result RemoveCheckpointId(int tourId, long checkpointId)
        {
            try
            {
                var tour = CrudRepository.Get(tourId);
                var tourCheckpoint = _tourCheckpointRepository.Get(checkpointId);
                if (tour.TourCheckpoints.Contains(tourCheckpoint))
                {
                    tour.TourCheckpoints.Remove(tourCheckpoint);
                    CrudRepository.Update(tour);
                    return Result.Ok();
                }
                return Result.Fail(FailureCode.InvalidArgument).WithError("Equipment ID not found in the list");
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }
        public Result RemoveEquipmentId(int tourId, long equipmentId)
        {
            try
            {
                var tour = CrudRepository.Get(tourId);
                var equipment = _equipmentRepository.Get(equipmentId);
                if (tour.Equipments.Contains(equipment))
                {
                    tour.Equipments.Remove(equipment);
                    CrudRepository.Update(tour); 
                    return Result.Ok();
                }
                return Result.Fail(FailureCode.InvalidArgument).WithError("Equipment ID not found in the list");
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }
        public Result UpdateCheckpointIds(int tourId, long checkpointId)
        {
            try
            {
                var tour = CrudRepository.Get(tourId);
                var tourCheckpoint = _tourCheckpointRepository.Get(checkpointId);
                // Ažuriraj listu checkpointa
                if (!tour.TourCheckpoints.Contains(tourCheckpoint))
                {
                    tour.TourCheckpoints.Add(tourCheckpoint);
                    CrudRepository.Update(tour);
                    return Result.Ok();
                }

                return Result.Fail(FailureCode.InvalidArgument).WithError("Checkpoint ID already exists in the list");
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

        public Result ArchiveTour(int tourId)
        {
            try
            {
                var tour = CrudRepository.Get(tourId);
                tour.SetArchived();
                CrudRepository.Update(tour);
                return Result.Ok();
            }
            catch(KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
            catch(ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

        public Result PublishTour(int tourId)
        {
            try
            {
                var tour = CrudRepository.Get(tourId);
                tour.setPublished();
                CrudRepository.Update(tour);
                return Result.Ok();
            }
            catch(KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
            catch(ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }
    }
}
