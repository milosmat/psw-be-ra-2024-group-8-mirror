using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class MembershipRequestDto
    {
        public long Id { get; set; }
        public int SenderId { get; set; }
        public int FollowerId { get; set; }
        public Status Status { get; set; }
        public long ClubId { get; set; }

    }

    public enum Status
    {
        None,
        Pending,
        Accepted,
        Rejected,
        Invited

    }
}
