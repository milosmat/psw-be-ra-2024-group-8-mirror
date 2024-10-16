using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos;
public enum DifficultyLevel { EASY = 1, MEDIUM = 2, HARD = 3 };
public enum TransportMode { WALK = 0, BIKE = 1, CAR = 3, BOAT = 4 };
public class TourPreferencesDto
{
    public int Id { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    public Dictionary<TransportMode, int> TransportPreferences { get; set; }
    public List<string> InterestTags { get; set; }
}
