namespace Explorer.Tours.API.Dtos
{
    public class TourDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Weight { get; set; }
        public string[] Tags { get; set; }
        public decimal? Price { get; set; }
        public List<long> equipmentIds {  get; set; }
        public List<long> TourCheckpointIds { get; set; }

    }
}
