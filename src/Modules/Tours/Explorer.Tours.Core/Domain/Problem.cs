using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain
{
    public class Problem : Entity
    {
        public Guid UserId { get; init; }
        public string TourId { get; init; }
        public string Category { get; init; }
        public string Priority { get; init; }
        public string Description { get; init; }
        public DateTime ReportedAt { get; init; }

    }
}
