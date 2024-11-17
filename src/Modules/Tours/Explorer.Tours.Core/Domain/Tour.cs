using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Tours.Core.Domain.ValueObjects;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain;
public class Tour : Entity
{
    public String Name { get; init; }
    public String Description { get; init; }
    public String Weight { get; init; }
    public String[] Tags { get; init; }
    public TourStatus Status { get; private set; }
    public Decimal? Price { get; init; }
    public long LengthInKm { get; init; }
    public DateTime PublishedDate { get; private set; }
    public DateTime ArchivedDate {  get; init; }
    public List<TravelTime> TravelTimes { get; init; }
    public List<Equipment> Equipments { get; init; }
    public List<TourCheckpoint> TourCheckpoints { get; init; }
    public long AuthorId { get; private set; }


    // Tour review
    public List<TourReview> TourReviews { get; private set; } = new List<TourReview>();

        public Tour() { }

        public Tour(string name, string description, string weight, string[] tags, long lengthInKm, DateTime publishedDate, DateTime archivedDate, long authorId)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid Name.");
            Name = name;
            Description = description;
            Weight = weight;
            Tags = tags;
            Status = TourStatus.DRAFT;
            Price = 0;
            LengthInKm = lengthInKm;
            PublishedDate = publishedDate;
            ArchivedDate = archivedDate;
            TravelTimes = new List<TravelTime>();
            Equipments = new List<Equipment>();
            TourCheckpoints = new List<TourCheckpoint>();
            AuthorId = authorId;
    }

        // Metode za upravljanje Equipments (vrednosni objekti)
        public Result AddEquipment(Equipment equipment)
        {
            if (Equipments.Contains(equipment))
                return Result.Fail("Equipment already exists.");

            Equipments.Add(equipment);
            return Result.Ok();
        }

        public Result RemoveEquipment(Equipment equipment)
        {
            if (!Equipments.Contains(equipment))
                return Result.Fail("Equipment not found.");

            Equipments.Remove(equipment);
            return Result.Ok();
        }

        // Metode za upravljanje TourCheckpoints (entiteti)
        public Result AddCheckpoint(TourCheckpoint checkpoint)
        {
            if (TourCheckpoints.Any(c => c.Id == checkpoint.Id))
                return Result.Fail("Checkpoint already exists.");

            TourCheckpoints.Add(checkpoint);
            return Result.Ok();
        }

        public Result RemoveCheckpoint(TourCheckpoint checkpoint)
        {
            if (!TourCheckpoints.Contains(checkpoint))
                return Result.Fail("Checkpoint not found.");

            TourCheckpoints.Remove(checkpoint);
            return Result.Ok();
        }

        // Metode za upravljanje TourReviews (entiteti)
        public Result AddTourReview(TourReview review)
        {
            if (TourReviews.Any(r => r.Personn.Id == review.Personn.Id && r.TourDate == review.TourDate))
                return Result.Fail("Review already exists for this tour date by the same person.");

            TourReviews.Add(review);
            return Result.Ok();
        }

        public Result RemoveTourReview(TourReview review)
        {
            if (!TourReviews.Contains(review))
                return Result.Fail("Review not found.");

            TourReviews.Remove(review);
            return Result.Ok();
        }

        public void SetArchived()
        {
            Status = TourStatus.ARCHIVED;
        }

    public Result setPublished()
    {

        if(Name != "" && Description != "" && Weight != "" && Tags.Length > 0 && 
            TourCheckpoints.Count >= 2 && TravelTimes.Count > 0) { 
            Status = TourStatus.PUBLISHED;
            PublishedDate = DateTime.UtcNow;
            return Result.Ok();
        }
        else
        {
            return Result.Fail("Tour must have name, description, travel time and al least 2 checkpoints.");
            
        }
        
    }
    public TourCheckpoint AddNewCheckpoint(TourCheckpoint checkpoint)
    {
        TourCheckpoints.Add(checkpoint);
        return checkpoint;
    }

    public TravelTime AddNewTravelTime(TravelTime travelTime)
    {
        TravelTimes.Add(travelTime);
        return travelTime;
    }
}

    public enum TourStatus
    {
        DRAFT, PUBLISHED, ARCHIVED
    }

