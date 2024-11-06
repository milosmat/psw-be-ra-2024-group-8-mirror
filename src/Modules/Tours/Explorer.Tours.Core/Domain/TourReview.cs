using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.Core.Domain;

namespace Explorer.Tours.Core.Domain
{
    public class TourReview : Entity
    {
        public int Rating { get; init; }
        public string Comment { get; init; }
        public Person Personn { get; init; }
        public DateTime TourDate { get; init; }
        public DateTime ReviewDate { get; init; }
        public string[] Images { get; init; }

        public TourReview() { }

        public TourReview(int rating, string comment, Person person, DateTime tourDate, DateTime reviewDate, string[] images)
        {
            if (rating < 1 || rating > 5) throw new ArgumentException("Rating must be between 1 and 5.");
            if (tourDate > reviewDate) throw new ArgumentException("Tour date cannot be later than the review date.");

            Rating = rating;
            Comment = comment;
            Personn = person;
            TourDate = tourDate;
            ReviewDate = reviewDate;
            Images = images ?? Array.Empty<string>();
        }
    }
}