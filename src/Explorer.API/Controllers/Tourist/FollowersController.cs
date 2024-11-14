using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;


[Authorize(Policy = "touristPolicy")]
[Route("api/followers")]
public class FollowersController : BaseApiController
{
    private readonly IFollowersService _followersService;

    public FollowersController(IFollowersService followService)
    {
        _followersService = followService;
    }

    [HttpGet]
    public ActionResult<PagedResult<FollowersDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _followersService.GetPaged(page, pageSize);
        return CreateResponse(result);
    }

    [HttpGet("{id}")]

    public ActionResult<FollowersDto> GetById(int id)
    {
        var result = _followersService.Get(id);
        return CreateResponse(result);
    }

    [HttpPost]
    public ActionResult<FollowersDto> Create([FromBody] FollowersDto followDto)
    {
        var currentUserId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
        followDto.followerId = currentUserId;

        var result = _followersService.Create(followDto);
        return CreateResponse(result);
    }

    [HttpPut("{id:int}")]
    public ActionResult<FollowersDto> Update([FromBody] FollowersDto followDto)
    {
        var currentUserId = int.Parse(User.Claims.First(c => c.Type == "id").Value);

        /*if (followDto.followerId != currentUserId)
        {
            return Forbid("You are not the follower.");
        }*/

        var result = _followersService.Update(followDto);
        return CreateResponse(result);
    }


    [HttpDelete("{id}")]

    public ActionResult Delete(int id)
    {
        var currentUser = int.Parse(User.Claims.First(c => c.Type == "id").Value);

        var result = _followersService.Get(id);

        if (result.IsFailed)
        {
            return NotFound(result.Errors?.FirstOrDefault()?.Message);
        }

        var existingFollow = result.Value;

        if (existingFollow.followerId != currentUser)
        {
            return Forbid();
        }

        var DeleteResult = _followersService.Delete(id);
        return CreateResponse(DeleteResult);
    }

    [HttpGet("follower/{followerId}")]
    public ActionResult<PagedResult<FollowersDto>> GetPagedByFollowerId(int followerId, [FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _followersService.GetPagedByFollowerId(followerId, page, pageSize);
        return CreateResponse(result);
    }



    [HttpDelete("follower/{followerId}/following/{followingId}")]
    public ActionResult DeleteByFollowerAndFollowingIds(int followerId, int followingId)
    {
        // Dobavljanje ID-a trenutnog korisnika iz tokena
        var currentUser = int.Parse(User.Claims.First(c => c.Type == "id").Value);

        // Proveravamo da li trenutni korisnik može da obriše vezu između followera i followinga
        var getFollowersResult = _followersService.GetPagedByFollowerId(currentUser, 1, int.MaxValue);

        if (getFollowersResult.IsFailed)
        {
            return NotFound("Could not find any followers for the current user.");
        }

        // Da li je pronađen zapis sa odgovarajućim followerId i followingId za trenutnog korisnika
        var followerMatch = getFollowersResult.Value.Results.FirstOrDefault(f => f.followerId == followerId && f.followingId == followingId);

        if (followerMatch == null)
        {
            return Forbid("You are not authorized to delete this following record.");
        }

        // Ako je pronađen odgovarajući zapis, pozivamo servis za brisanje
        var deleteResult = _followersService.DeleteByFollowerAndFollowingIds(followerId, followingId);
        return CreateResponse(deleteResult);
    }


}
