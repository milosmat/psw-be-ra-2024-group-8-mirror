using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos;

public class BundleTourDTO
{
    public long TourId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public long BundleId { get; set; }
}
