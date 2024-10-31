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
        public TourStatus Status { get; private set; }
        public decimal? Price { get; set; }
        public DateTime PublishedDate { get; private set; }
        public DateTime ArchivedDate { get; private set; }
        public List<EquipmentDto> Equipments {  get; set; }
        public List<TourCheckpointDto> TourCheckpoint { get; set; }
        //public List<TravelTime> TravelTimes { get; set; }
    }
}
public enum TourStatus
{
    DRAFT, PUBLISHED, ARCHIVED
}