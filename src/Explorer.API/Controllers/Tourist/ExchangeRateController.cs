using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Route("api/tourist/exchange")]
    public class ExchangeRateController : BaseApiController
    {
        private readonly IExchangeRateService _exchangeRateService;

        public ExchangeRateController(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        [HttpGet("convert")]
        public async Task<ActionResult<ExchangeRateDto>> Convert(
            [FromQuery] decimal amount,
            [FromQuery] string fromCurrency,
            [FromQuery] string toCurrency)
        {
            var result = await _exchangeRateService.ConvertCurrencyAsync(amount, fromCurrency, toCurrency);
            return CreateResponse(result);
        }

        [HttpGet("currencies")]
        public async Task<ActionResult<Dictionary<string, string>>> GetAllCurrencies()
        {
            var result = await _exchangeRateService.GetAllCurrenciesAsync();
            return CreateResponse(result);
        }
    }
}
