using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;

namespace Explorer.Stakeholders.API.Public
{
    public interface IMembershipRequestService
    {
        PagedResult<MembershipRequestDto> GetPagedByClub(long clubId, int page, int pageSize);
        MembershipRequestDto Create(long clubId, MembershipRequestDto newMembershipRequest);
        MembershipRequestDto Update(long clubId, MembershipRequestDto updatedMembershipRequest);
        void Delete(long membershipRequestId, long clubId);
        MembershipRequestDto GetById(long membershipRequestId, long clubId);

    }
}
