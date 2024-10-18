using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain;

public enum DifficultyLevel { EASY = 1, MEDIUM = 2, HARD = 3 };
public class TourPreferences : Entity
{
    public DifficultyLevel Difficulty { get; private set; }
    public int WalkRating { get; private set; }
    public int BikeRating { get; private set; }
    public int CarRating { get; private set; }
    public int BoatRating { get; private set; }
    public List<string> InterestTags { get; private set; }

    public TourPreferences(DifficultyLevel difficulty, int walkRating, int bikeRating, int carRating, int boatRating, List<string> interestTags) 
    {
        Validate(difficulty,interestTags);
        Difficulty = difficulty;
        InterestTags = interestTags;

        WalkRating = ValidateRating(walkRating);
        BikeRating = ValidateRating(bikeRating);
        CarRating = ValidateRating(carRating);
        BoatRating = ValidateRating(boatRating);        
    }

    private void Validate(DifficultyLevel difficulty, List<string> interestTags)
    {
        if (difficulty == null)
        {
            throw new ArgumentNullException(nameof(difficulty), "Difficulty cannot be null.");
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
    private int ValidateRating(int rating)
    {
        if (rating < 0 || rating > 3)
        {
            throw new ArgumentOutOfRangeException("Rating must be between 0 and 3.", nameof(rating));
        }
        return rating;
    }
}

