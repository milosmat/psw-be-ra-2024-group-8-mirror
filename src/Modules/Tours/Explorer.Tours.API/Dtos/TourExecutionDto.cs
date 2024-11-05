using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{   public class TourExecutionDto
    {
        public int TourId { get; set; }
        public int UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TourExecutionStatus Status { get; set; } // IN_PROGRESS, COMPLETED, ABANDONED
    }

    public enum TourExecutionStatus
    {
        IN_PROGRESS,
        COMPLETED,
        ABANDONED
    }
}
