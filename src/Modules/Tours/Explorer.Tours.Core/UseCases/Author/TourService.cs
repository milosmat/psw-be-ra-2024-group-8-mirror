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
        public TourService(ICrudRepository<Tour> repository, IMapper mapper) : base(repository, mapper) 
        {
        }
        public Result<List<long>> GetCheckpointIds(int tourId)
        {
            try
            {
                var tourResult = CrudRepository.Get(tourId);
                return Result.Ok(tourResult.TourCheckpointIds);
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
                return Result.Ok(tourResult.equipmentIds);
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

                if (!tour.TourCheckpointIds.Contains(checkpointId))
                {
                    tour.TourCheckpointIds.Add(checkpointId);
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

                if (!tour.equipmentIds.Contains(equipmentId))
                {
                    tour.equipmentIds.Add(equipmentId);
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

                if (tour.TourCheckpointIds.Contains(checkpointId))
                {
                    tour.TourCheckpointIds.Remove(checkpointId);
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

                if (tour.equipmentIds.Contains(equipmentId))
                {
                    tour.equipmentIds.Remove(equipmentId);
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

    }
}
