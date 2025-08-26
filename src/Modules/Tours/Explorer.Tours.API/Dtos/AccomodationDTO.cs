using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class AccomodationDTO
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string[]? Images { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Category {  get; set; }
        public string? ContactNumber {  get; set; }
        public string? City { get; set; }



    }
}
