using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain.Clubs;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class MembershipRequestService: IMembershipRequestService
    {
        private readonly IClubRepository _clubRepository;
        public IMapper _mapper;

        public MembershipRequestService(IClubRepository clubRepository, IMapper mapper)
        {
            _clubRepository = clubRepository;
            _mapper = mapper;
        }

        public PagedResult<MembershipRequestDto> GetPagedByClub(long clubId, int page, int pageSize)
        {
            var club = _clubRepository.GetClubWithMembershipRequests(clubId);
            if (club == null)
            {
                throw new KeyNotFoundException($"Club with ID {clubId} not found.");
            }

            var allMembershipRequests = club.MembershipRequests?.ToList();
            int totalCount = allMembershipRequests.Count;

            if (page != 0 || pageSize != 0)
            {
                allMembershipRequests
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }

            var membershipRequestDtos = _mapper.Map<List<MembershipRequestDto>>(allMembershipRequests);
            return new PagedResult<MembershipRequestDto>(membershipRequestDtos, totalCount);

        }

        public MembershipRequestDto Create(long clubId, MembershipRequestDto newRequest)
        {
            var club = _clubRepository.GetClubWithMembershipRequests(clubId);
            if (club == null)
            {
                throw new KeyNotFoundException($"Club with ID {clubId} not found.");
            }

            club.AddMembershipRequest(_mapper.Map<MembershipRequest>(newRequest));
            _clubRepository.Update(club);
            var lastMembershipRequest = club.MembershipRequests.Last();
            newRequest.Id = lastMembershipRequest.Id;
            return newRequest;

        }

        public MembershipRequestDto Update(long clubId, MembershipRequestDto updatedMembershipRequestDto)
        {
            var club = _clubRepository.GetClubWithMembershipRequests(clubId);
            if (club == null)
            {
                throw new KeyNotFoundException($"Club with ID {clubId} not found.");
            }

            var updatedMemRequest = club.UpdateMembershipRequest(_mapper.Map<MembershipRequest>(updatedMembershipRequestDto));
            _clubRepository.Update(club);
            return _mapper.Map<MembershipRequestDto>(updatedMemRequest);
        }

        public void Delete(long clubId, long membershipRequestId)
        {
            var club = _clubRepository.GetClubWithMembershipRequests(clubId);
            if (club == null)
            {
                throw new KeyNotFoundException($"Club with ID {clubId} not found.");
            }
            club.DeleteMembershipRequest(membershipRequestId);
            _clubRepository.Update(club);
        }

        public MembershipRequestDto GetById(long membershipRequestId, long clubId)
        {
            var club = _clubRepository.GetClubWithMembershipRequests(clubId);
            if (club == null)
            {
                throw new KeyNotFoundException($"Club with ID {clubId} not found.");
            }

            var memRequest = club.MembershipRequests.FirstOrDefault(mr => mr.Id == membershipRequestId);
            if (memRequest == null)
            {
                throw new KeyNotFoundException($"Membership request with ID {membershipRequestId} not found.");
            }

            return _mapper.Map<MembershipRequestDto>(memRequest);

        }

        public bool IsTouristInvited(long clubId, long touristId)
        {
            // Proveravamo da li postoji zahtev sa statusom "Invited"
            var club = _clubRepository.Get(clubId);
            var request = _clubRepository.GetClubWithMembershipRequests(clubId).MembershipRequests
                .FirstOrDefault(r =>
                    r.ClubId == clubId &&
                    r.OwnerId == club.OwnerId &&
                    r.SenderId == touristId &&
                    r.Status == Domain.Clubs.MemRequestStatus.Invited); // Status je "Invited"

            return request != null; // Ako postoji, vraćamo true, inače false
        }


    }
}
