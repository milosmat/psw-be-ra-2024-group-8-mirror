using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class TouristPositionDto
    {
        public long Id { get; set; }             
        public long TouristId { get; set; }        

        public MapLocationDto CurrentLocation { get; set; }

        public class MapLocationDto
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }
}
