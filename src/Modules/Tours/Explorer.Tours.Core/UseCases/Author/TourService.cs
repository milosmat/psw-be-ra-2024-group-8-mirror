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
        private readonly IMapper _mapper;
        public TourService(ICrudRepository<Tour> repository, IMapper mapper,
            ICrudRepository<TourCheckpoint> tourCheckpointRepository,
            ICrudRepository<Equipment> equipmentRepository) : base(repository, mapper)
        {
            _mapper = mapper;
            _tourCheckpointRepository = tourCheckpointRepository;
            _equipmentRepository = equipmentRepository;
        }

        public Result AddEquipment(int tourId, EquipmentDto equipmentDto)
        {
            var equipment = _mapper.Map<Equipment>(equipmentDto);
            var tour = CrudRepository.Get(tourId);
            if (tour == null)
                return Result.Fail("Tour not found.");

            var result = tour.AddEquipment(equipment);
            if (result.IsSuccess)
                CrudRepository.Update(tour);

            return result;
        }

        public Result RemoveEquipment(int tourId, EquipmentDto equipmentDto)
        {
            var equipment = _mapper.Map<Equipment>(equipmentDto);
            var tour = CrudRepository.Get(tourId);
            if (tour == null)
                return Result.Fail("Tour not found.");

            var result = tour.RemoveEquipment(equipment);
            if (result.IsSuccess)
                CrudRepository.Update(tour);

            return result;
        }

        public Result AddCheckpoint(int tourId, TourCheckpointDto checkpointDto)
        {
            var checkpoint = _mapper.Map<TourCheckpoint>(checkpointDto);
            var tour = CrudRepository.Get(tourId);
            if (tour == null)
                return Result.Fail("Tour not found.");

            var result = tour.AddCheckpoint(checkpoint);
            if (result.IsSuccess)
                CrudRepository.Update(tour);

            return result;
        }

        public Result RemoveCheckpoint(int tourId, TourCheckpointDto checkpointDto)
        {
            var checkpoint = _mapper.Map<TourCheckpoint>(checkpointDto);
            var tour = CrudRepository.Get(tourId);
            if (tour == null)
                return Result.Fail("Tour not found.");

            var result = tour.RemoveCheckpoint(checkpoint);
            if (result.IsSuccess)
                CrudRepository.Update(tour);

            return result;
        }

        public Result<PagedResult<TourCheckpointDto>> GetPagedCheckpoint(int page, int pageSize)
        {
            var checkpoints = _tourCheckpointRepository.GetPaged(page, pageSize);
            if (checkpoints == null)
            {
                return Result.Fail("No checkpoints found.");
            }

            var checkpointDtos = checkpoints.Results.Select(cp => _mapper.Map<TourCheckpointDto>(cp)).ToList();
            var pagedResult = new PagedResult<TourCheckpointDto>(checkpointDtos, checkpoints.TotalCount);
            return Result.Ok(pagedResult);
        }

        public Result<TourCheckpointDto> CreateCheckpoint(TourCheckpointDto checkpointDto)
        {
            var checkpoint = _mapper.Map<TourCheckpoint>(checkpointDto);
            var createdCheckpoint = _tourCheckpointRepository.Create(checkpoint);

            if (createdCheckpoint == null)
            {
                return Result.Fail("Failed to create checkpoint.");
            }

            return Result.Ok(_mapper.Map<TourCheckpointDto>(createdCheckpoint));
        }

        public Result<TourCheckpointDto> GetCheckpoint(int id)
        {
            var checkpoint = _tourCheckpointRepository.Get(id);
            if (checkpoint == null)
            {
                return Result.Fail("Checkpoint not found.");
            }

            return Result.Ok(_mapper.Map<TourCheckpointDto>(checkpoint));
        }

        public Result<TourCheckpointDto> UpdateCheckpoint(int id, TourCheckpointDto checkpointDto)
        {
            var checkpointObj = _tourCheckpointRepository.Get(id);
            _mapper.Map(checkpointDto, checkpointObj);
            var updatedCheckpoint = _tourCheckpointRepository.Update(checkpointObj);

            if (updatedCheckpoint == null)
            {
                return Result.Fail("Failed to update checkpoint.");
            }

            return Result.Ok(_mapper.Map<TourCheckpointDto>(updatedCheckpoint));
        }

        public Result DeleteCheckpoint(int id)
        {
            try
            {
                _tourCheckpointRepository.Delete(id);
                return Result.Ok();
            }
            catch (KeyNotFoundException)
            {
                return Result.Fail("Checkpoint not found.");
            }
        }

        public Result<PagedResult<EquipmentDto>> GetPagedEquipment(int page, int pageSize)
        {
            var equipments = _equipmentRepository.GetPaged(page, pageSize);
            if (equipments == null)
            {
                return Result.Fail("No equipment found.");
            }

            var equipmentDtos = equipments.Results.Select(e => _mapper.Map<EquipmentDto>(e)).ToList();
            var pagedResult = new PagedResult<EquipmentDto>(equipmentDtos, equipments.TotalCount);
            return Result.Ok(pagedResult);
        }

        public Result<EquipmentDto> CreateEquipment(EquipmentDto equipmentDto)
        {
            var equipment = _mapper.Map<Equipment>(equipmentDto);

            var createdEquipment = _equipmentRepository.Create(equipment);

            if (createdEquipment == null)
            {
                return Result.Fail("Failed to create equipment.");
            }

            return Result.Ok(_mapper.Map<EquipmentDto>(createdEquipment));
        }

        public Result<EquipmentDto> UpdateEquipment(int id, EquipmentDto equipmentDto)
        {
            var equipmentObj = _equipmentRepository.Get(id);
            _mapper.Map(equipmentDto, equipmentObj);
            var updatedEquipment = _equipmentRepository.Update(equipmentObj);

            if (updatedEquipment == null)
            {
                return Result.Fail("Failed to update equipment.");
            }

            return Result.Ok(_mapper.Map<EquipmentDto>(updatedEquipment));
        }

        public Result<EquipmentDto> GetEquipment(int id)
        {
            var equipment = _equipmentRepository.Get(id);
            if (equipment == null)
            {
                return Result.Fail("Equipment not found.");
            }

            return Result.Ok(_mapper.Map<EquipmentDto>(equipment));
        }

        public Result DeleteEquipment(int id)
        {
            try
            {
                _equipmentRepository.Delete(id);
                return Result.Ok();
            }
            catch (KeyNotFoundException)
            {
                return Result.Fail("Equipment not found.");
            }
        }
    }
}
