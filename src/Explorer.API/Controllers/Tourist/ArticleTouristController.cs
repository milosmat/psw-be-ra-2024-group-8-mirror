using Explorer.Tours.API.Public.Author;
using Explorer.Tours.Core.UseCases.Author;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Route("api/tourist/articles")]
    public class ArticleTouristController : BaseApiController
    {
        private readonly IArticleService _articlesService;

        public ArticleTouristController(IArticleService articlesService)
        {
            _articlesService = articlesService;
        }

        [HttpGet("published")]
        public IActionResult GetPublishedArticles()
        {
            var result = _articlesService.GetPublishedArticles();

            // Proveravamo rezultat
            if (result.IsFailed)
            {
                // Ako je došlo do greške, vraćamo grešku sa statusnim kodom 400 (BadRequest)
                return BadRequest(result.Errors);
            }

            // Ako je uspešno, vraćamo podatke sa statusnim kodom 200 (OK)
            return Ok(result.Value);
        }

    }
}
