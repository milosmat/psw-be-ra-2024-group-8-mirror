using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Tours.Core.Domain.ValueObjects;
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
    public TourStatus Status { get; init; }
    public Decimal? Price { get; init; }
    public long LengthInKm { get; init; }
    public DateTime PublishedDate { get; init; }
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
}

public enum TourStatus
{
    DRAFT, PUBLISHED, ARCHIVED
}

