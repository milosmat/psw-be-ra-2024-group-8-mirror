using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain;


public class BundleTour : Entity
{
    public long TourId { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public long BundleId { get; set; } 

    public BundleTour(long tourId, string name, decimal price, long bundleId)
    {
        if (tourId <= 0) throw new ArgumentException("Invalid Tour ID.");
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid Name.");
        if (price < 0) throw new ArgumentException("Price cannot be negative.");

        TourId = tourId;
        Name = name;
        Price = price;
        BundleId = bundleId;
    }
}
