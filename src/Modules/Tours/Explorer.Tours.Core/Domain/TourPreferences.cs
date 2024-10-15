using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain;

public enum DifficultyLevel { EASY = 1, MEDIUM = 2, HARD = 3 };
public enum TransportMode { WALK = 0, BIKE = 1, CAR = 3, BOAT = 4 };
public class TourPreferences : Entity
{
    public DifficultyLevel Difficulty { get; private set; }
    public Dictionary<TransportMode, int> TransportPreferences { get; private set; } 
    public List<string> InterestTags { get; private set; }

    public TourPreferences(DifficultyLevel difficulty, Dictionary<TransportMode, int> transportPreferences, List<string> interestTags) 
    {
        Validate(difficulty, transportPreferences, interestTags);

        Difficulty = difficulty;
        TransportPreferences = transportPreferences;
        InterestTags = interestTags;
    }

    private void Validate(DifficultyLevel difficulty, Dictionary<TransportMode, int> transportPreferences, List<string> interestTags)
    {
        if (difficulty == null)
        {
            throw new ArgumentNullException(nameof(difficulty), "Difficulty cannot be null.");
        }

        if (transportPreferences == null || transportPreferences.Count == 0)
        {
            throw new ArgumentException("Transport preferences cannot be null or empty.", nameof(transportPreferences));
        }

        foreach (var preference in transportPreferences)
        {
            if (preference.Value < 0 || preference.Value > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(transportPreferences), "Transport preference rating must be between 0 and 3.");
            }
        }

        if (interestTags == null || interestTags.Count == 0)
        {
            throw new ArgumentException("Interest tags cannot be null or empty.", nameof(interestTags));
        }

        foreach (var tag in interestTags)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                throw new ArgumentException("Interest tags cannot contain null or empty strings.", nameof(interestTags));
            }
        }
    }
}

