using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/clubs")]

public class ClubController : BaseApiController
{
    private readonly IClubService _clubService;

    public ClubController(IClubService clubService)
    {
        _clubService = clubService;
    }

    [HttpGet]
    public ActionResult<PagedResult<ClubDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _clubService.GetPaged(page, pageSize);
        return CreateResponse(result);
    }

    [HttpGet("{id}")]

    public ActionResult<ClubDto> GetById(int id)
    {
        var result = _clubService.Get(id);
        return CreateResponse(result);
    }

    [HttpPost]
    public ActionResult<ClubDto> Create([FromBody] ClubDto clubDto)
    {
        var currentUserId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
        clubDto.OwnerId = currentUserId;

        var result = _clubService.Create(clubDto);
        return CreateResponse(result);
    }

    [HttpPut("{id}")]

    public ActionResult<ClubDto> Update([FromBody] ClubDto clubDto, int id)
    {
        var currentUser = int.Parse(User.Claims.First(c => c.Type == "id").Value);

        var result = _clubService.Get(id);

        if (result.IsFailed)
        {
            return NotFound(result.Errors.FirstOrDefault()?.Message);
        }
        var existingClub = result.Value;

        if (existingClub.OwnerId != currentUser)
        {
            return Forbid();
        }

        clubDto.Id = id;

        var updateResult = _clubService.Update(clubDto);
        return CreateResponse(updateResult);
    }

    [HttpDelete("{id}")]

    public ActionResult Delete(int id)
    {
        var currentUser = int.Parse(User.Claims.First(c => c.Type == "id").Value);

        var result = _clubService.Get(id);

        if (result.IsFailed)
        {
            return NotFound(result.Errors?.FirstOrDefault()?.Message);
        }

        var existingClub = result.Value;

        if (existingClub.OwnerId != currentUser)
        {
            return Forbid();
        }

        var DeleteResult = _clubService.Delete(id);
        return CreateResponse(DeleteResult);
    }

}
