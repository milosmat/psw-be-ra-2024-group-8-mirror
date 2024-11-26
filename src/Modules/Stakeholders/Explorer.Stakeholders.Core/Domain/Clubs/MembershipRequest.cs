using System;
﻿using Explorer.BuildingBlocks.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain.Clubs
{
    public class MembershipRequest : Entity
    {
        public int SenderId { get; set; }
        public int OwnerId { get; set; }
        
        public MemRequestStatus Status { get; set; }
        public long ClubId { get; set; }


        public MembershipRequest() { }
        public MembershipRequest(int senderId, int ownerId, MemRequestStatus status)
        {
            if (senderId <= 0) throw new ArgumentException("Sender ID must be positive.", nameof(senderId));
            if (ownerId <= 0) throw new ArgumentException("Follower ID must be positive.", nameof(ownerId));
            
            SenderId = senderId;
            OwnerId = ownerId;
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


