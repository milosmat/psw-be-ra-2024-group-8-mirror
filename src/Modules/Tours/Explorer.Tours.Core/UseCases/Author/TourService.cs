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
    }
}
