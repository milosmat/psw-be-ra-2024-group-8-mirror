using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Author;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author;

[Route("api/author/bundles")]
public class BundleController : BaseApiController
{
    private readonly IBundleService _bundleService;

    public BundleController(IBundleService bundleService)
    {
        _bundleService = bundleService;
    }

    [HttpGet]
    public ActionResult<PagedResult<BundleDTO>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _bundleService.GetPaged(page, pageSize);
        return CreateResponse(result);
    }

    [HttpGet("{id:int}")]
    public ActionResult<BundleDTO> GetById(int id)
    {
        var result = _bundleService.Get(id);
        return CreateResponse(result);
    }


    [HttpGet("published")]
    public ActionResult<List<BundleDTO>> GetPublishedBundles()
    {
        var result = _bundleService.GetPublishedBundles();
        return CreateResponse(result);
    }

    [HttpGet("allBundles")]
    public ActionResult<List<BundleDTO>> GetAllBundles()
    {
        var result = _bundleService.GetAllBundles();
        return CreateResponse(result);
    }


    [HttpPost]
    public ActionResult<BundleDTO> Create([FromBody] BundleDTO bundleDto)
    {
        try
        {
            BundleDTO result = _bundleService.Create(bundleDto);

            if (result != null)
            {
                return Ok(result);
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
    public ActionResult<BundleDTO> Update(int id, [FromBody] BundleDTO bundleDto)
    {
        bundleDto.Id = id;
        var result = _bundleService.Update(bundleDto);
        return CreateResponse(result);
    }

    [HttpPost("{bundleId:int}/add-tour")]
    public ActionResult AddTour(int bundleId, [FromBody] BundleTourDTO bundleTourDto)
    {
        var result = _bundleService.AddTour(bundleId, bundleTourDto);
        return CreateResponse(result);
    }



    [HttpDelete("bundleTours/{id:int}")]
    public ActionResult DeleteTour(int id)
    {
        var result = _bundleService.DeleteTour(id);
        return CreateResponse(result);
    }



    [HttpGet("{bundleId:int}/tours")]
    public ActionResult<PagedResult<BundleTourDTO>> GetPagedTours(int bundleId, [FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _bundleService.GetPagedTours(bundleId, page, pageSize);
        return CreateResponse(result);
    }

    [HttpPost("{bundleId:int}/set-status")]
    public ActionResult SetStatus(int bundleId, [FromQuery] int status)
    {
        var result = _bundleService.SetStatus(bundleId, status);
        return CreateResponse(result);
    }

    [HttpPost("{bundleId:int}/archive")]
    public ActionResult ArchiveBundle(int bundleId)
    {
        var result = _bundleService.ArchiveBundle(bundleId);
        return CreateResponse(result);
    }

    [HttpPost("{bundleId:int}/check-tours-status")]
    public ActionResult CheckToursStatus(long bundleId)
    {
        var result = _bundleService.CheckToursStatus(bundleId);

        if (result.IsSuccess)
        {
            return Ok(new { message = "All tours are published, and the bundle status is now 'PUBLISHED'." });
        }
        else
        {
            return BadRequest(new { message = result.Errors.First().Message });
        }
    }

    [HttpGet("bundleTours/{bundleId}")]
    public IActionResult GetBundleTourIds(int bundleId)
    {
        var bundle = _bundleService.GetById(bundleId); 
        if (bundle == null)
        {
            return NotFound("Bundle not found.");
        }

        var tourIds = bundle.Tours.Select(t => t.TourId).ToList();
        return Ok(tourIds);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var result = _bundleService.Delete(id);
        return CreateResponse(result);
    }


}
