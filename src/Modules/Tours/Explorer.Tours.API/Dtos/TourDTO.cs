using Explorer.Tours;
namespace Explorer.Tours.API.Dtos
{
    public class TourDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Weight { get; set; }
        public string[] Tags { get; set; }
        public int Status { get; set; }
        public decimal? Price { get; set; }
        public long? LengthInKm { get; set; }
        public DateTime? PublishedDate { get; set; }
        public DateTime? ArchivedDate { get; set; }
        public List<EquipmentDto>? Equipments {  get; set; }
        public List<TourCheckpointDto>? TourCheckpoints { get; set; }
        public List<TravelTimeDTO>? TravelTimes { get; set; }
        public List<DailyAgendaDTO>? DailyAgendas {  get; set; }
        public long AuthorId { get;  set; }
    }
}