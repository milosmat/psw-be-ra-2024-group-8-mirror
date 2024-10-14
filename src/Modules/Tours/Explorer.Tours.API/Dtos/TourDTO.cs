using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class TourDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Weight { get; set; }
        public string[] Tags { get; set; }
        public decimal? Price { get; set; }
    }
}
