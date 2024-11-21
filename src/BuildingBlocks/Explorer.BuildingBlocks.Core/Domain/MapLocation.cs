using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class MapLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public MapLocation() { }

        public MapLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public override bool Equals(object obj)
        {
            if (obj is MapLocation other)
            {
                return Latitude == other.Latitude && Longitude == other.Longitude;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Latitude, Longitude);
        }
    }
}
