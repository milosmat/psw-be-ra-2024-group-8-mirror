using Explorer.Stakeholders.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class TourReviewDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }

        public string Comment { get; set; }
         public Person Personn { get; init; }
        public DateTime TourDate { get; set; }
        public DateTime ReviewDate { get; set; }
        public string[] Images { get; set; }
    }
}
