using Explorer.BuildingBlocks.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain;


public enum BundleStatus
{
    DRAFT, PUBLISHED, ARCHIVED
}
public class Bundle : Entity
{
    public string Name { get; init; }
    private decimal TotalToursPrice; 
    public decimal CustomPrice { get; private set; } 
    public DateTime? PublishedDate { get; private set; }
    public DateTime? ArchivedDate { get; private set; }
    public List<BundleTour> Tours { get; private set; }
    public long AuthorId { get; private set; }
    public BundleStatus Status { get; private set; }

    public decimal TotalToursPriceCalculated => TotalToursPrice;

    public Bundle(string name, long authorId)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid Name.");
        Name = name;
        AuthorId = authorId;
        Tours = new List<BundleTour>();
        Status = BundleStatus.DRAFT;
        TotalToursPrice = 0;
        CustomPrice = 0;
    }

    public Result AddTour(BundleTour tour)
    {
        if (Tours.Any(c => c.TourId == tour.TourId))
            return Result.Fail("Tour already exists in the bundle.");

        Tours.Add(tour);
        UpdateTotalToursPrice();
        return Result.Ok();
    }

    public Result RemoveTour(BundleTour tour)
    {
        if (!Tours.Contains(tour))
            return Result.Fail("Tour not found in the bundle.");

        Tours.Remove(tour);
        UpdateTotalToursPrice();
        return Result.Ok();
    }


    public void SetCustomPrice(decimal price)
    {
        if (price < 0) throw new ArgumentException("Price cannot be negative.");
        CustomPrice = price;
    }

    public void Publish()
    {
        Status = BundleStatus.PUBLISHED;
        PublishedDate = DateTime.UtcNow;
    }

    public void Archive()
    {
        Status = BundleStatus.ARCHIVED;
        ArchivedDate = DateTime.UtcNow;
    }

    private void UpdateTotalToursPrice()
    {
        TotalToursPrice = Tours.Sum(item => item.Price);
    }

    public void UpdateStatus(BundleStatus newStatus)
    {
        Status = newStatus;
    }

    public decimal GetTotalPrice()
    {
        return TotalToursPrice;
    }


}
