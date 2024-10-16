using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain;

public class TourCheckpoint : Entity
{
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Image { get; private set; }

    public TourCheckpoint(double latitude, double longitude, string name, string description, string image)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty");
        if (string.IsNullOrEmpty(description)) throw new ArgumentException("Description cannot be null or empty");
        if (string.IsNullOrEmpty(image)) throw new ArgumentException("Image cannot be null or empty");

        Latitude = latitude;
        Longitude = longitude;
        Name = name;
        Description = description;
        Image = image;
    }
}

