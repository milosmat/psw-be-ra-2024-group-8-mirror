using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain;


public class Followers : Entity
{
    public int followerId { get; init; } // ID korisnika koji prati
    public int followingId { get; init; } // ID korisnika kojeg prati

    public Followers() { }

    public Followers(int followerId, int followingId)
    {
        this.followerId = followerId;
        this.followingId = followingId;
        Validate();
    }

    private void Validate()
    {
        if (followerId == 0) throw new ArgumentException("Invalid follower Id");
        if (followingId == 0) throw new ArgumentException("Invalid followeing Id");
    }
}
