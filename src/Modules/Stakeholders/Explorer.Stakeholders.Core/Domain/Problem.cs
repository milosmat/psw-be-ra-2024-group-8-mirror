using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain
{
    public class Problem : Entity
    {

        public int UserId { get; init; }
        public int TourId { get; init; }
        public string Category { get; init; }
        public string Priority { get; init; }
        public string Description { get; init; }
        public Boolean IsResolved { get; init; }
        public DateTime ReportedAt { get; init; }
    }
}
