using Explorer.Tours.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Dtos
{
    public class CheckTouristsRequestDto
    {
        public TouristPositionDto TouristLocation { get; set; }
        public List<TouristPositionDto> AllTouristLocations { get; set; }
    }

}
