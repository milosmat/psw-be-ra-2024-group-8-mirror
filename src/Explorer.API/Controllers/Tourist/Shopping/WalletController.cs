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

        [HttpPut("add-adventure-coins")]
        public ActionResult<WalletDTO> AddAdventureCoins([FromBody] AdventureCoinsRequest request)
        {
            Console.WriteLine("ID walleta: " + request.IdWallet +
                              ", Id Administratora: " + request.IdAdministrator +
                              ", Adventure Coins: " + request.AdventureCoins +
                              ", Description: " + request.Description);

            var result = _walletService.AddTransaction(request.IdWallet, request.IdAdministrator, request.AdventureCoins, request.Description);

            return CreateResponse(result);
        }

        [HttpPut("subtrac-adventure-coins")]
        public ActionResult<WalletDTO> SubtractAdventureCoins([FromBody] AdventureCoinsRequest request)
        {
            Console.WriteLine("ID walleta: " + request.IdWallet +
                              ", Id Administratora: " + request.IdAdministrator +
                              ", Adventure Coins: " + request.AdventureCoins +
                              ", Description: " + request.Description);

            var result = _walletService.SubtractTransaction(request.IdWallet, request.IdAdministrator, request.AdventureCoins, request.Description);

            return CreateResponse(result);
        }




    }
}
