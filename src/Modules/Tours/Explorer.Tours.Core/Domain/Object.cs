using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class Object : Entity
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public string Image { get; init; }
        public ObjectCategory Category { get; init; }


        public Object(string name, string description, string image, string objectCategory)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid Name.");
            if (string.IsNullOrWhiteSpace(objectCategory) ||
    !Enum.TryParse(objectCategory, true, out ObjectCategory parsedCategory))
            {
                throw new ArgumentException("Invalid Category.");
            }
            Name = name;
            Description = description;
            Image = image;
            Category = parsedCategory;
        }
    }
}

public enum ObjectCategory
{
    TOILET, PARKING, RESTAURANT, OTHER
}
