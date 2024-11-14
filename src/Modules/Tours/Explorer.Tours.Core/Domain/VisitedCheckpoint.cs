using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class VisitedCheckpoint : Entity
    {
        public int CheckpointId { get; private set; }
        public DateTime VisitTime { get; private set; }
        public string Secret {  get; private set; }
        public VisitedCheckpoint(int checkpointId, DateTime visitTime, string secret)
        {
            CheckpointId = checkpointId;
            VisitTime = visitTime;
            Secret = secret;
        }
        
    }
}
