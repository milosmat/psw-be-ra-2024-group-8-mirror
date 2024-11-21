using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain.Clubs
{
    public class MembershipRequest: Entity
    {
        public int SenderId { get; set; }
        public int FollowerId { get; private set; }
        
        public MemRequestStatus Status { get; private set; }
        public long ClubId { get; set; }


        public MembershipRequest() { }
        public MembershipRequest(int senderId, int followerId, MemRequestStatus status)
        {
            if (senderId <= 0) throw new ArgumentException("Sender ID must be positive.", nameof(senderId));
            if (followerId <= 0) throw new ArgumentException("Follower ID must be positive.", nameof(followerId));


            SenderId = senderId;
            FollowerId = followerId;
            Status = status;
        }

        internal void SetClubId(long clubId)
        {
            ClubId = clubId;
        }
    }
    

    public enum MemRequestStatus
    {
        None,
        Pending,
        Accepted,
        Rejected,
        Invited
    }
}

