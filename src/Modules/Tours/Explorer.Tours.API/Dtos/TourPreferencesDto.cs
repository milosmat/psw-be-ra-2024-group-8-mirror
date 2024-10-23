using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos;
public enum DifficultyLevel { EASY = 1, MEDIUM = 2, HARD = 3 };
public class TourPreferencesDto
{
    public int Id { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    public int WalkRating { get; set; }
    public int BikeRating { get; set; }
    public int CarRating { get; set; }
    public int BoatRating { get; set; }
    public List<string> InterestTags { get; set; }
}
