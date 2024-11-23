using Explorer.Blog.API.Dtos;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/club/{clubId}/memshiprequest")]
    public class MembershipRequestContoller: BaseApiController
    {
        private readonly IMembershipRequestService _membershipRequestService;
        public MembershipRequestContoller(IMembershipRequestService membershipRequestService)
        {
            _membershipRequestService = membershipRequestService;
        }

        [HttpGet]
        public ActionResult<PagedResult<MembershipRequestDto>> GetAll([FromRoute] long clubId, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _membershipRequestService.GetPagedByClub(clubId, page, pageSize);
            if (result == null || !result.Results.Any())
            {
                return NotFound("No membership request found for the specified club.");
            }

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<MembershipRequestDto> Create([FromRoute] long clubId, [FromBody] MembershipRequestDto newMemshipRequest)
        {
            try
            {
                var createdMemshipRequest = _membershipRequestService.Create(clubId, newMemshipRequest);
                return CreatedAtAction(nameof(Create), new { clubId, memshipRequestId = createdMemshipRequest.Id }, createdMemshipRequest);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult<CommentDto> Update([FromRoute] long clubId, long id, [FromBody] MembershipRequestDto updatedMembershipRequest)
        {
            try
            {
                var existingMemRequest = _membershipRequestService.GetById(id, clubId);
                if (existingMemRequest == null)
                {
                    return NotFound($"Membership request with ID {id} not found.");
                }

                var result = _membershipRequestService.Update(clubId, updatedMembershipRequest);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete([FromRoute] long clubId, long id)
        {
            try
            {
                _membershipRequestService.Delete(clubId, id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpGet("is-tourist-invited/{touristId}")]
        public ActionResult<bool> IsTouristInvited([FromRoute] long clubId, [FromRoute] long touristId)
        {
            try
            {
                // Pozivamo servis da proverimo da li postoji zahtev za člana sa statusom "Invited"
                var isInvited = _membershipRequestService.IsTouristInvited(clubId, touristId);

                return Ok(isInvited); // Vraća true/false
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Ako postoji neki problem, vratiti NotFound
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Generalni greške
            }
        }

    }
}
