using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class VisitedCheckpointDTO
    {
        public int CheckpointId { get; set; }
        public DateTime VisitTime { get; set; }

        public VisitedCheckpointDTO(int checkpointId, DateTime visitTime)
        {
            CheckpointId = checkpointId;
            VisitTime = visitTime;
        }
        public VisitedCheckpointDTO() { }
    }
}
