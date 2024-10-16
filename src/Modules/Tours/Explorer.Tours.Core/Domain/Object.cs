using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    internal class Object : Entity
    {
        public String Name { get; init; }
        public String Description { get; init; }
        public String Image { get; private set; }
        public ObjectCategory Category { get; private set; }


        public Object(string name, string description, string image, ObjectCategory objectCategory)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid Name.");
            Name = name;
            Description = description;
            Image = image;
            Category = objectCategory;
        }
    }
}

public enum ObjectCategory
{
    TOILET, PARKING, RESTAURANT, OTHER
}
