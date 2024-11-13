using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Author;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using FluentResults;
using Explorer.Stakeholders.Core.Domain;

namespace Explorer.API.Controllers.Tourist

{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/tours")]
    public class TouristTourController : BaseApiController
    {
        private readonly ITourService _tourService;


        public TouristTourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        [HttpGet]
        public ActionResult<PagedResult<TourDTO>> GetAllTours()
        {
            var result = _tourService.GetAllTours();
            return CreateResponse(result);
        }

    }
}

