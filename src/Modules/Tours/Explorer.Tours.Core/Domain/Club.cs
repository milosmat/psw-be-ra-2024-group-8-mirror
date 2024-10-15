using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain;


public class Club : Entity
{
    public string Name { get; init; }
    public string Description { get; init; }
    public string Photo { get; init; }
    public long OwnerId { get; init; }


    public Club(string name, string description, string photo, long owner)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid Name.");
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Invalid Description.");
        if (string.IsNullOrWhiteSpace(photo)) throw new ArgumentException("Invalid Photo.");
        if (owner == 0) throw new ArgumentException("Invalid OwnerId.");


        Name = name;
        Description = description;
        Photo = photo;
        OwnerId = owner;
    }
}
