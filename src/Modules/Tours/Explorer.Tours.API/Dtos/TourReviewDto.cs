using Explorer.Stakeholders.Core.Domain;

namespace Explorer.Tours.API.Dtos
{
    public class TourReviewDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }

        public string Comment { get; set; }
        public Person Personn { get; init; }
        public DateTime TourDate { get; set; }
        public DateTime ReviewDate { get; set; }
        public string[] Images { get; set; }
        public TourDTO Tour { get; set; }
    }
}
