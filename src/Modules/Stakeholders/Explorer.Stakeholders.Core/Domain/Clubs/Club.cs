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
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Photo { get; private set; }
        public int OwnerId { get; private set; }
        public List<MembershipRequest> MembershipRequests { get; private set; } = new List<MembershipRequest>();

        public Club(){}
        public Club(long id, string name, string description, string photo, int ownerId)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Invalid Name.");
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Invalid Description.");
            if (string.IsNullOrWhiteSpace(photo)) throw new ArgumentException("Invalid Photo.");
            if (ownerId == 0) throw new ArgumentException("Invalid OwnerId.");

            Id = id;
            Name = name;
            Description = description;
            Photo = photo;
            OwnerId = ownerId;
            MembershipRequests = new List<MembershipRequest>();
        }

        public void AddMembershipRequest(MembershipRequest newMembershipRequest)
        {
            if (newMembershipRequest == null)
            {
                throw new ArgumentNullException(nameof(newMembershipRequest));
            }

            newMembershipRequest.SetClubId(this.Id);
            MembershipRequests.Add(newMembershipRequest);
        }

        public void DeleteMembershipRequest(long membershipRequestId)
        {
            var request = MembershipRequests.FirstOrDefault(mr => mr.Id == membershipRequestId);
            if (request == null)
            {
                throw new KeyNotFoundException($"Membership request with ID {membershipRequestId} not found.");
            }

            MembershipRequests.RemoveAll(mr => mr.Id == membershipRequestId);
        }
    }
}
