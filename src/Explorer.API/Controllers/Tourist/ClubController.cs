using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
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
        try
        {
            var result = _clubService.GetPaged(page, pageSize);

            if (result == null || result.Results.Count == 0)
            {
                return NotFound("No clubs found for the specified page.");
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while fetching data: {ex.Message}");
        }
    }
    
    [HttpGet("{id:int}")]
    public ActionResult<ClubDto> GetById(int id)
    {
        try
        {
            var result = _clubService.Get(id);

            if (result != null)
            {
                return Ok(result); 
            }
            else
            {
                return NotFound($"Club with ID {id} not found.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }


    [HttpPost]
    public ActionResult<ClubDto> Create([FromBody] ClubDto clubDto)
    {
        try
        {
            ClubDto result = _clubService.Create(clubDto);

            if (result != null)
            {
                return Ok(result);// CreatedAtAction(nameof(Create), new { name = result.Name }, result);
            }
            else
            {
                return BadRequest("The club could not be created due to invalid data.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }

    }

    [HttpPut("{id:int}")]
    public ActionResult<ClubDto> Update([FromBody] ClubDto clubDto)
    {
        try
        {
            ClubDto result = _clubService.Update(clubDto);
            return Ok(result);

        }
        catch (Exception ex)
        {
            // Return a 500 error for unexpected issues
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }


    [HttpDelete("{id:int}")]

    public ActionResult Delete(int id)
    {
        var club = _clubService.Get(id);

        if (club == null)
        {
            return NotFound($"Club with ID {id} not found.");
        }

        _clubService.Delete(id);

        return Ok($"Club with ID {id} has been successfully deleted.");
    }

}


