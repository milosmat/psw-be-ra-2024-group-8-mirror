using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class ProblemDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string TourId { get; set; }
        public string Category { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public DateTime ReportedAt { get; set; }

    }
}
