using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using System.Collections.Generic;

namespace Explorer.Tours.API.Public.Author
{
    public interface ITourService
    {
        Result<PagedResult<TourDTO>> GetPaged(int page, int pageSize);
        Result<TourDTO> Get(int id);
        Result<TourDTO> Create(TourDTO tourDto);
        Result<TourDTO> Update(TourDTO tourDto);
        Result Delete(int id);

        // Metode za dobijanje ID-ova povezane opreme i tačaka
        //Result<List<long>> GetEquipmentIds(int tourId);
        //Result<List<long>> GetCheckpointIds(int tourId);

        // Metode za upravljanje opremom, tačkama i vremenom putovanja u okviru agregata Tour
        Result AddEquipment(int tourId, EquipmentDto equipment); 
        Result RemoveEquipment(int tourId, EquipmentDto equipment);

        Result AddCheckpoint(int tourId, TourCheckpointDto checkpoint); 
        Result RemoveCheckpoint(int tourId, TourCheckpointDto checkpoint);

        //Result AddTravelTime(int tourId, TravelTimeDto travelTime); 
        //Result RemoveTravelTime(int tourId, TravelTimeDto travelTime);
    }
}
