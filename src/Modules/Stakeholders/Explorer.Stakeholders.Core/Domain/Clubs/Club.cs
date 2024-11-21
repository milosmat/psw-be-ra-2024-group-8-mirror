using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.Clubs
{
    public class Club : Entity
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public string Photo { get; init; }
        public int OwnerId { get; init; }
        public List<MembershipRequest> Requests { get; private set; }

        public Club() { }
        public Club(string name, string description, string photo, int ownerId, List<MembershipRequest> requests)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid Name.");
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Invalid Description.");
            if (string.IsNullOrWhiteSpace(photo)) throw new ArgumentException("Invalid Photo.");
            if (ownerId == 0) throw new ArgumentException("Invalid OwnerId.");


            Name = name;
            Description = description;
            Photo = photo;
            OwnerId = ownerId;
            Requests = new List<MembershipRequest>();
        }
    }
}
