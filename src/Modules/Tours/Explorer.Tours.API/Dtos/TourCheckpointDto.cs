using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos;

public class TourCheckpointDto
{
    public int Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string CheckpointName { get; set; }
    public string CheckpointDescription { get; set; }
    public string Image { get; set; }
    public long TourId { get; set; }
}

