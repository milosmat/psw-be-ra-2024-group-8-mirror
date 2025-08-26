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
        public List<MembershipRequest> MembershipRequests { get; private set; }
        public List<Message> Messages { get; private set; }

        public Club() { }
        public Club(long id, string name, string description, string photo, int ownerId, List<MembershipRequest> requests, List<Message> messages)
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
            MembershipRequests = requests ?? new List<MembershipRequest>();
            Messages = messages;
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
                throw new KeyNotFoundException($"Membership message with ID {membershipRequestId} not found.");
            }

            MembershipRequests.RemoveAll(mr => mr.Id == membershipRequestId);
        }

        public MembershipRequest UpdateMembershipRequest(MembershipRequest updatedMembershipRequest)
        {
            var requestToUpdate = MembershipRequests.FirstOrDefault(mr => mr.Id == updatedMembershipRequest.Id);
            if (requestToUpdate == null)
            {
                throw new KeyNotFoundException("Membership message to update not found.");
            }

            requestToUpdate.Status = updatedMembershipRequest.Status;
            return requestToUpdate;
        }

        public void AddMessage(Message newMessage)
        {
            if (newMessage == null)
            {
                throw new ArgumentNullException(nameof(newMessage));
            }

            //newMessage.SetClubId(this.Id);
            Messages.Add(newMessage);
        }

        public void DeleteMessage(long messageId)
        {
            var message = Messages.FirstOrDefault(mr => mr.Id == messageId);
            if (message == null)
            {
                throw new KeyNotFoundException($"Membership message with ID {messageId} not found.");
            }

            Messages.RemoveAll(mr => mr.Id == messageId);
        }

        public Message UpdateMesagge(Message updateMessage)
        {
            var messageToUpdate = Messages.FirstOrDefault(mr => mr.Id == updateMessage.Id);
            if (messageToUpdate == null)
            {
                throw new KeyNotFoundException("Membership message to update not found.");
            }

            messageToUpdate.Content = updateMessage.Content;
            return updateMessage;
        }
    }
}
