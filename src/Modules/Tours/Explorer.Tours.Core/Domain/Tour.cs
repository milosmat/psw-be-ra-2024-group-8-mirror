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

    public Tour(string name, string description, string weight, string[] tags, long lengthInKm, DateTime publishedDate, DateTime archivedDate)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid Name.");
        Name = name;
        Description = description;
        Weight = weight;
        Tags = tags;
        Status = TourStatus.DRAFT;
        Price = new Decimal(0);
        LengthInKm = lengthInKm;
        PublishedDate = publishedDate;
        ArchivedDate = archivedDate;
        TravelTimes = new List<TravelTime>();
        Equipments = new List<Equipment>();
        TourCheckpoints = new List<TourCheckpoint>();
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

