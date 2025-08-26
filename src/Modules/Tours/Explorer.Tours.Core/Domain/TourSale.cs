using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain;

public class TourSale : Entity
{
    public List<int> Tours { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public bool Active { get; private set; }
    public double Discount { get; private set; }
    public long AuthorId { get; private set; }

    public TourSale(List<int> tours, DateTime startDate, DateTime endDate, double discount, bool active, long authorId)
    {
        if (discount <= 0) throw new ArgumentException("Invalid discount.");
        if (authorId <= 0) throw new ArgumentException("Invalid authorId.");
        if (endDate <= startDate) throw new ArgumentException("End date must be after start date.");
        if (tours == null || !tours.Any()) throw new ArgumentException("At least one tour must be specified.");

        Tours = tours;
        StartDate = startDate;
        EndDate = endDate;
        Discount = discount;
        Active = active;
        AuthorId = authorId;
    }
}
