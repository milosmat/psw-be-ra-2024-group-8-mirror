using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class Accomodation : Entity
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public string[]? Images { get; init; }
        public double Latitude { get; init; }
        public double Longitude { get; init; }
        public AccomodationCategory Category { get; init; }
        public string? ContactNumber { get; init; }
        public string? City { get; init; }
        public List<Tour> Tours { get; init; }

    }
}
public enum AccomodationCategory
{
    APPARTMENT, HOTEL, HOUSE
}
