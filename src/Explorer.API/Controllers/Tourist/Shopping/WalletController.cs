using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public.Tourist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;


namespace Explorer.API.Controllers.Tourist.Shopping
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/wallet")]
    public class WalletController : BaseApiController
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet("{id:int}")]
        public ActionResult<WalletDTO> GetById(int id)
        {
            Console.WriteLine("ID koji sam prosledio od klijenta : " + id);
            var result = _walletService.Get(id);
            return CreateResponse(result);
        }

    }
}
