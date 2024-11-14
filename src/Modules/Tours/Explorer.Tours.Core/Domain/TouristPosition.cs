using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class TouristPosition : Entity
    {
        public long TouristId { get; private set; } 
        public MapLocation CurrentLocation { get; private set; }

        public TouristPosition(int touristId, MapLocation initialLocation)
        {
            TouristId = touristId;
            CurrentLocation = initialLocation;
        }
        private TouristPosition() { }

        public void UpdateLocation(MapLocation newLocation)
        {
            CurrentLocation = newLocation;
        }
    }
}
