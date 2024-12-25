using Explorer.Blog.API.Public;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Author;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.UseCases.Author;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author
{
    [Route("api/author/articles")]
    public class ArticleController : BaseApiController
    {
        private readonly IArticleService _articlesService;

        public ArticleController(IArticleService articlesService)
        {
            _articlesService = articlesService;
        }

        [HttpGet("{tourId}/article")]
        public IActionResult GetArticleByTourId(long tourId)
        {
            var articleResult = _articlesService.GetArticleByTourId(tourId);

            if (articleResult.IsFailed)
            {
                return NotFound(articleResult.Errors.FirstOrDefault()?.Message);
            }

            return Ok(articleResult.Value);
        }

        [HttpGet("author/{authorId}")]
        public IActionResult GetArticlesByAuthorId(long authorId)
        {
            var result = _articlesService.GetArticlesByAuthorId(authorId);

            if (result.IsFailed)
            {
                // Vraćamo grešku ako nije pronađen nijedan članak za autora
                return NotFound(result.Errors.FirstOrDefault()?.Message);
            }

            // Ako su članci pronađeni, vraćamo listu članaka
            return Ok(result.Value);
        }

        [HttpPut("{articleId}")]
        public IActionResult UpdateArticle(long articleId, [FromBody] ArticleDTO articleDto)
        {
            var result = _articlesService.UpdateArticle(articleId, articleDto);

            if (result.IsFailed)
            {
                // Vraćamo grešku ako članku nije moguće ažurirati
                return BadRequest(result.Errors.FirstOrDefault()?.Message);
            }

            // Ako je članak uspešno ažuriran, vraćamo OK status
            return NoContent();  // 204 No Content - uspešan zahtev bez vraćanja tela odgovora
        }

        [HttpGet]
        public IActionResult GetAllArticles()
        {
            var result = _articlesService.GetAllArticles();

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

        [HttpGet("author/{authorId}/unpublished")]
        public IActionResult GetUnpublishedArticlesByAuthorId(long authorId)
        {
            var result = _articlesService.GetUnpublishedArticlesByAuthorId(authorId);

            if (result.IsFailed)
            {
                return NotFound(result.Errors.FirstOrDefault()?.Message);
            }

            return Ok(result.Value);
        }

        // Metoda za vraćanje objavljenih članaka jednog autora
        [HttpGet("author/{authorId}/published")]
        public IActionResult GetPublishedArticlesByAuthorId(long authorId)
        {
            var result = _articlesService.GetPublishedArticlesByAuthorId(authorId);

            if (result.IsFailed)
            {
                return NotFound(result.Errors.FirstOrDefault()?.Message);
            }

            return Ok(result.Value);
        }

        [HttpPut("{articleId}/publish")]
        public IActionResult PublishArticle(long articleId)
        {
            var result = _articlesService.PublishArticle(articleId);

            if (result.IsFailed)
            {
                // Vraćamo grešku ako nije moguće objaviti članak
                return BadRequest(result.Errors.FirstOrDefault()?.Message);
            }

            // Ako je članak uspešno objavljen, vraćamo OK status
            return NoContent();  // 204 No Content - uspešan zahtev bez vraćanja tela odgovora
        }


    }
}
