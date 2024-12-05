using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos;

public class TourSaleDto
{
    public int Id { get; set; }
    public List<int> Tours { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool Active { get; set; }
    public double Discount { get; set; }
    public long AuthorId { get; set; }
}
