using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class AuthorStatsDto
    {
        public long AuthorId { get; set; }
        public double ResolvedPercentage { get; set; }
        public double ClosedPercentage { get; set; }
        public double UnresolvedPercentage { get; set; }
    }
}
