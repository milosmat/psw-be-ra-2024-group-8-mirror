using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain;

public class TourCheckpoint : Entity
{
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public string CheckpointName { get; private set; }
    public string CheckpointDescription { get; private set; }
    public string Image { get; private set; }
    public long TourId { get; set; }

    public TourCheckpoint(double latitude, double longitude, string checkpointName, string checkpointDescription, string image)
    {
        if (string.IsNullOrEmpty(checkpointName)) throw new ArgumentException("Name cannot be null or empty");
        if (string.IsNullOrEmpty(checkpointDescription)) throw new ArgumentException("Description cannot be null or empty");
        if (string.IsNullOrEmpty(image)) throw new ArgumentException("Image cannot be null or empty");

        Latitude = latitude;
        Longitude = longitude;
        CheckpointName = checkpointName;
        CheckpointDescription = checkpointDescription;
        Image = image;
    }
}

