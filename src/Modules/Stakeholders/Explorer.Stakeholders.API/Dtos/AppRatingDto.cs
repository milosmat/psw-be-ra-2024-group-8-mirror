using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Stakeholders;

namespace Explorer.Stakeholders.API.Dtos
{
    public class AppRatingDto
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime TimeCreated { get; set; }
        public int UserPostedId { get; set; }
    }
}
