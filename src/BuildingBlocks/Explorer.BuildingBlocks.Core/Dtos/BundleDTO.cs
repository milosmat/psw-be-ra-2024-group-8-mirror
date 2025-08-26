using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.BuildingBlocks.Core.Dtos
{
    public class BundleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal CustomPrice { get; set; }
        public decimal TotalToursPriceCalculated { get; set; }
        public DateTime? PublishedDate { get; set; }
        public DateTime? ArchivedDate { get; set; }
        public long AuthorId { get; set; }
        public int Status { get; set; }
        public List<BundleTourDTO> Tours { get; set; } = new();
    }
}
