using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.Domain;
using FluentResults;
using System;
using System.Linq;

namespace Explorer.Tours.Core.UseCases.Tourist
{
    public class TouristPositionService : ITouristPositionService
    {
        private readonly ICrudRepository<TouristPosition> _repository;

        public TouristPositionService(ICrudRepository<TouristPosition> repository)
        {
            _repository = repository;
        }

        public Result<TouristPositionDto> GetPosition(int touristId)
        {
            try
            {
                // Dohvati sve pozicije i filtriraj na osnovu TouristId
                var pagedResult = _repository.GetPaged(1, int.MaxValue);
                var touristPosition = pagedResult.Results.FirstOrDefault(p => p.TouristId == touristId);

                if (touristPosition == null)
                    return Result.Fail<TouristPositionDto>("Tourist position not found");

                // Ručno mapiranje TouristPosition u TouristPositionDto
                var dto = new TouristPositionDto
                {
                    Id = touristPosition.Id,
                    TouristId = touristPosition.TouristId,
                    CurrentLocation = new TouristPositionDto.MapLocationDto
                    {
                        Latitude = touristPosition.CurrentLocation.Latitude,
                        Longitude = touristPosition.CurrentLocation.Longitude
                    }
                };

                return Result.Ok(dto);
            }
            catch (Exception ex)
            {
                return Result.Fail<TouristPositionDto>(ex.Message);
            }
        }

        public Result<TouristPositionDto> SetPosition(int touristId, double latitude, double longitude)
        {
            try
            {
                // Dohvati sve pozicije i filtriraj na osnovu TouristId
                var pagedResult = _repository.GetPaged(1, int.MaxValue);
                var touristPosition = pagedResult.Results.FirstOrDefault(p => p.TouristId == touristId);
                var newLocation = new MapLocation(latitude, longitude);

                if (touristPosition == null)
                {
                    touristPosition = new TouristPosition(touristId, newLocation);
                    _repository.Create(touristPosition);
                }
                else
                {
                    touristPosition.UpdateLocation(newLocation);
                    _repository.Update(touristPosition);
                }

                // Ručno mapiranje ažuriranog entiteta u DTO
                var updatedDto = new TouristPositionDto
                {
                    Id = touristPosition.Id,
                    TouristId = touristPosition.TouristId,
                    CurrentLocation = new TouristPositionDto.MapLocationDto
                    {
                        Latitude = touristPosition.CurrentLocation.Latitude,
                        Longitude = touristPosition.CurrentLocation.Longitude
                    }
                };

                return Result.Ok(updatedDto);
            }
            catch (Exception ex)
            {
                return Result.Fail<TouristPositionDto>(ex.Message);
            }
        }
    }
}
