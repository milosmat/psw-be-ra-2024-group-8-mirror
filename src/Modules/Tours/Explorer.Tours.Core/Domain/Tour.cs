using Explorer.BuildingBlocks.Core.Domain;
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
    public List<long> equipmentIds { get; init; }
    public List<long> TourCheckpointIds { get; init; }

    public Tour(string name, string description, string weight, string[] tags)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid Name.");
        Name = name;
        Description = description;
        Weight = weight;
        Tags = tags;
        Status = TourStatus.DRAFT;
        Price = new Decimal(0);
        equipmentIds = new List<long>();
        TourCheckpointIds = new List<long>();
    }

    public void SetArchived()
    {
        Status = TourStatus.ARCHIVED;
    }

    public void setPublished()
    {
        Status = TourStatus.PUBLISHED;
    }
}

public enum TourStatus
{
    DRAFT, PUBLISHED, ARCHIVED
}

