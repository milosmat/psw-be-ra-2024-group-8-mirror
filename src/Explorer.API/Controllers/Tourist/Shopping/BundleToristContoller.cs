using Explorer.Payments.API.Public.Tourist;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Author;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.Shopping
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/bundles")]
    public class BundleToristContoller : BaseApiController
    {

        private readonly IBundleService _bundleService;
        private readonly IShoppingCartService _shoppingCartService;

        public BundleToristContoller(IBundleService bundleService, IShoppingCartService shoppingCartService)
        {
            _bundleService = bundleService;
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet("publishedboundles")]
        public ActionResult<List<BundleDTO>> GetPublishedBundles()
        {
            var result = _bundleService.GetPublishedBundles();
            return CreateResponse(result);
        }





    }
}
