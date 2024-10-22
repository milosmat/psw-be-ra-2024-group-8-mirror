using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases.Tourist
{
    public class TouristEquipmentService : /*CrudService<TouristEquipmentDTO, TouristEquipment>,*/ ITouristEquipmentService
    {
        private readonly ITouristEquipmentRepository _touristEquipmentRepository;
        private readonly IMapper _mapper;

        public TouristEquipmentService(ITouristEquipmentRepository touristEquipmentRepository, IMapper mapper)
        {
            _touristEquipmentRepository = touristEquipmentRepository;
            _mapper = mapper;
        }

        public Result<TouristEquipmentDTO> Create(TouristEquipmentDTO touristEquipmentDto)
        {
            try
            {
                var touristEquipment = _mapper.Map<TouristEquipment>(touristEquipmentDto);

                // Kreiraj TouristEquipment u bazi
                var createdTouristEquipment = _touristEquipmentRepository.Create(touristEquipment);

                // Mapiraj nazad u DTO i vrati rezultat
                var createdTouristEquipmentDto = _mapper.Map<TouristEquipmentDTO>(createdTouristEquipment);
                return Result.Ok(createdTouristEquipmentDto);
            }
            catch (Exception e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }

        public Result Delete(int id)
        {
            try
            {
                var touristEquipment = _touristEquipmentRepository.Get(id);

                if (touristEquipment == null)
                {
                    return Result.Fail(FailureCode.NotFound).WithError("TouristEquipment not found.");
                }

                _touristEquipmentRepository.Delete(id);
                return Result.Ok();
            }
            catch (Exception e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }

        public Result<TouristEquipmentDTO> FindByTouristAndEquipment(long touristId, long equipmentId)
        {
            try
            {
                var touristEquipment = _touristEquipmentRepository.GetByTouristAndEquipment(touristId, equipmentId);
                if (touristEquipment == null)
                {
                    return Result.Fail(FailureCode.NotFound).WithError("TouristEquipment not found.");
                }
                var touristEquipmentDto = _mapper.Map<TouristEquipmentDTO>(touristEquipment);
                return Result.Ok(touristEquipmentDto);
            }
            catch (Exception e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }

        public Result<TouristEquipmentDTO> Get(int id)
        {
            try
            {
                var touristEquipment = _touristEquipmentRepository.Get(id);

                if (touristEquipment == null)
                {
                    return Result.Fail(FailureCode.NotFound).WithError("TouristEquipment not found.");
                }

                var touristEquipmentDto = _mapper.Map<TouristEquipmentDTO>(touristEquipment);
                return Result.Ok(touristEquipmentDto);
            }
            catch (Exception e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }

    }
}
