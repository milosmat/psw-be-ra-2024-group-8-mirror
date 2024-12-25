using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Games.API.Dtos;
using Explorer.Games.API.Public.Tourist;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Explorer.API.Controllers.Tourist
{
    [Route("api/games")]
    [ApiController]  // Oznaka za kontroler koji koristi API pristup
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        // Konstruktor koji prima GameService kao zavisnost
        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        // Get metodu za pretragu igre po ID-u
        [HttpGet("{id:long}")]
        public ActionResult<GameDTO> GetGameById(long id)
        {
            try
            {
                var result = _gameService.Get(id);
                return Ok(result.Value); // Ako je uspešno, vraćamo igru
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message); // Ako je igra nepostojeća, vraćamo grešku
            }
        }

        // Metoda za kreiranje nove igre
        [HttpPost]
        public ActionResult<GameDTO> CreateGame([FromBody] GameDTO gameDto)
        {
            try
            {
                if (gameDto == null)
                {
                    return BadRequest("GameDTO cannot be null.");
                }

                var result = _gameService.Create(gameDto);
                return CreateResponse(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // Metoda za ažuriranje postojećih podataka o igri
        [HttpPut]
        public ActionResult<GameDTO> UpdateGame([FromBody] GameDTO gameDto)
        {
            try
            {
                var result = _gameService.Update(gameDto);
                return CreateResponse(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // Metoda za brisanje igre prema ID-u
        [HttpDelete("{id:long}")]
        public ActionResult DeleteGame(long id)
        {
            var result = _gameService.Delete(id);
            return CreateResponse(result);
        }

        // Metoda za paginaciju igara
        [HttpGet("paged")]
        public ActionResult<PagedResult<GameDTO>> GetPagedGames([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = _gameService.GetPaged(page, pageSize);
                return Ok(result.Value); // Ako je uspešno, vraćamo paginirane igre
            }
            catch (Exception e)
            {
                return BadRequest(e.Message); // Ako dođe do greške, vraćamo grešku
            }
        }

        // Metoda za dodavanje rezultata za igru
        [HttpPost("{gameId:long}/add-score")]
        public ActionResult AddScoreToGame(long gameId, [FromBody] AddScoreRequest addScoreRequest)
        {
            if (addScoreRequest.Score <= 0)
            {
                return BadRequest("Score must be a positive number.");
            }

            var result = _gameService.AddNewScore(gameId, addScoreRequest.UserId, addScoreRequest.Score);
            return CreateResponse(result);
        }

        // Helper method for creating ActionResults based on FluentResults
        private ActionResult<T> CreateResponse<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.Value); // Vrati 200 OK sa podacima
            }
            return BadRequest(result.Errors.Select(e => e.Message).ToArray()); // Ako nije uspešno, vrati greške
        }

        private ActionResult CreateResponse(Result result)
        {
            if (result.IsSuccess)
            {
                return Ok(); // Vrati 200 OK
            }
            return BadRequest(result.Errors.Select(e => e.Message).ToArray()); // Ako nije uspešno, vrati greške
        }
    }

    // DTO za dodavanje rezultata (score)
    public class AddScoreRequest
    {
        public long UserId { get; set; }
        public double Score { get; set; }
    }
}
