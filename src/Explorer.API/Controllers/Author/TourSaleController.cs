using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Author;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author
{
    [Route("api/author/tourSale")]
    public class TourSaleController : BaseApiController
    {
        private readonly ITourSaleService _tourSaleService;
        

        public TourSaleController(ITourSaleService tourSaleService)
        {
            _tourSaleService = tourSaleService;
            
        }

        [HttpPost("activateSales")]
        public ActionResult ActivateSales([FromBody] List<TourSaleDto> tourSales)
        {
            if (tourSales != null && tourSales.Count > 0)
            {
                foreach (var sale in tourSales)
                {
                    if (!sale.Active && sale.StartDate.Date <= DateTime.Today)
                    {
                        sale.Active = true;
                        _tourSaleService.Update(sale);                   
                    }
                }
                return Ok("Sales successfully activated.");
            }
            return Ok("Sales successfully activated.");
        }

        [HttpPost("deactivateSales")]
        public ActionResult DectivateSales([FromBody] List<TourSaleDto> tourSales)
        {
            if (tourSales != null && tourSales.Count > 0)
            {
                foreach (var sale in tourSales)
                {
                    if (sale.Active && sale.EndDate.Date < DateTime.Today)
                    {
                        sale.Active = false;
                        _tourSaleService.Update(sale);
                    }
                }
                return Ok("Sales successfully activated.");
            }
            return Ok("Sales successfully activated.");
        }

        [HttpGet]
        public ActionResult<PagedResult<TourSaleDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourSaleService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpPost]
        public ActionResult<TourSaleDto> Create([FromBody] TourSaleDto tourSale)
        {
            var result = _tourSaleService.Create(tourSale);
            return CreateResponse(result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<TourSaleDto> Update([FromBody] TourSaleDto tourSale)
        {
            var result = _tourSaleService.Update(tourSale);
            return CreateResponse(result);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var result = _tourSaleService.Delete(id);
            return CreateResponse(result);
        }

        [HttpGet("{id:int}")]
        public ActionResult<TourSaleDto> GetById(int id)
        {
            var result = _tourSaleService.Get(id);
            return CreateResponse(result);
        }
    }
}
