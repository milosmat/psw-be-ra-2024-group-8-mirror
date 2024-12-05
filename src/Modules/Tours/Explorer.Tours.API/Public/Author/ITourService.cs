using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Result<PagedResult<TourCheckpointDto>> GetPagedCheckpoint(int page, int pageSize);
        Result<TourCheckpointDto> CreateCheckpoint(TourCheckpointDto checkpoint);
        Result<TourCheckpointDto> GetCheckpoint(int id);
        Result<TourCheckpointDto> UpdateCheckpoint(int id, TourCheckpointDto checkpoint);
        Result DeleteCheckpoint(int id);

        Result<PagedResult<EquipmentDto>> GetPagedEquipment(int page, int pageSize);
        Result<EquipmentDto> CreateEquipment(EquipmentDto equipment);
        Result<EquipmentDto> UpdateEquipment(int id, EquipmentDto equipment);
        Result<EquipmentDto> GetEquipment(int id);
        Result DeleteEquipment(int id);
      
        Result<PagedResult<TourReviewDto>> GetPagedReviews(int tourId, int page, int pageSize);
        Result<TourReviewDto> AddReview(int tourId, TourReviewDto reviewDto);
        Result<TourReviewDto> UpdateReview(int reviewId, TourReviewDto reviewDto);
        Result DeleteReview(int reviewId);
      
        Result ArchiveTour(int tourId);
        Result<List<long>> GetEquipmentIds(int tourId);
        //Result AddEquipmentId(int tourId, long equipmentId);
        //Result RemoveEquipmentId(int tourId, long equipmentId);
        Result<TourCheckpointDto> AddNewCheckpoint(long tourId, TourCheckpointDto tourCheckpoint);
        Result<TravelTimeDTO> AddNewTravelTime(long tourId, TravelTimeDTO travelTime);
        Result<DailyAgendaDTO> AddNewDailyAgenda(long tourId, DailyAgendaDTO dailyAgenda);
        Result PublishTour(int tourId);
        Result<List<TourDTO>> GetAllTours();

        Result<List<long>> GetCheckpointIds(int tourId);
        //Result AddCheckpointId(int tourId, long checkpointId);
        //Result RemoveCheckpointId(int tourId, long checkpointId);
        //Result UpdateCheckpointIds(int id, long checkpointIds);

        Result<List<TourDTO>> GetToursByAuthorId(int authorId);
    }
}
