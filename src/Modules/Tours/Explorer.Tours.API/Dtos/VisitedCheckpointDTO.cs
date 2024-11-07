using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class VisitedCheckpointDTO
    {
        public int Id { get; set; }
        public int CheckpointId { get; set; }
        public DateTime VisitTime { get; set; }
        public string Secret { get; set; }
        public VisitedCheckpointDTO(int checkpointId, DateTime visitTime, string secret)
        {
            CheckpointId = checkpointId;
            VisitTime = visitTime;
            Secret = secret;
        }
        public VisitedCheckpointDTO() { }
    }
}
