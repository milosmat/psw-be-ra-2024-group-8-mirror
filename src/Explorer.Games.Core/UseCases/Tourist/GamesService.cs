using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Games.API.Dtos;
using Explorer.Games.Core.Domain.RepositoryInterfaces;
using Explorer.Games.Core.Domain;
using FluentResults;
using Explorer.Payments.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using Explorer.Games.API.Public.Tourist;
using Explorer.Payments.API.Public.Tourist;
using Explorer.Payments.API.Dtos;

namespace Explorer.Games.Core.UseCases.Tourist
{
    public class GameService : IGameService
    {
        private readonly IGamesRepository _gameRepository;
        private readonly IMapper _mapper;
        private readonly ICouponService _couponService;
        public GameService(IGamesRepository gameRepository, IMapper mapper, ICouponService couponService)
        {
            _gameRepository = gameRepository;
            _mapper = mapper;
            _couponService = couponService;
        }

        public Result<GameDTO> Create(GameDTO gameDto)
        {
            try
            {
                if (gameDto == null)
                {
                    return Result.Fail("GameDto cannot be null.");
                }

                var game = _mapper.Map<Game>(gameDto);
                var createdGame = _gameRepository.Create(game);
                if (createdGame == null)
                {
                    return Result.Fail("Creation failed.");
                }

                var createdGameDto = _mapper.Map<GameDTO>(createdGame);
                return Result.Ok(createdGameDto);
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }

        public Result<GameDTO> Update(GameDTO gameDto)
        {
            try
            {
                if (gameDto == null)
                {
                    return Result.Fail("GameDto cannot be null.");
                }

                var game = _gameRepository.Get(gameDto.Id);
                if (game == null)
                {
                    return Result.Fail("Game not found.");
                }

                game.Highscore = gameDto.Highscore;
                game.Scores = _mapper.Map<List<GameScore>>(gameDto.Scores);

                var updatedGame = _gameRepository.Update(game);
                var updatedGameDto = _mapper.Map<GameDTO>(updatedGame);
                return Result.Ok(updatedGameDto);
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }

        public Result<GameDTO> Get(long id)
        {
            try
            {
                var game = _gameRepository.Get(id);
                if (game == null)
                {
                    return Result.Fail("Game not found.");
                }

                var gameDto = _mapper.Map<GameDTO>(game);
                return Result.Ok(gameDto);
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }

        public Result<PagedResult<GameDTO>> GetPaged(int page, int pageSize)
        {
            try
            {
                var games = _gameRepository.GetPaged(page, pageSize);
                if (games == null)  // Prvo proveravamo da li je kolekcija prazna
                {
                    return Result.Fail("No games found.");
                }

                var gameDtos = _mapper.Map<List<GameDTO>>(games);

                // Kreiranje PagedResult objekta sa prosleđenim rezultatima
                var pagedResult = new PagedResult<GameDTO>(gameDtos, gameDtos.Count);

                return Result.Ok(pagedResult);
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }

        public Result Delete(long id)
        {
            try
            {
                var game = _gameRepository.Get(id);
                if (game == null)
                {
                    return Result.Fail("Game not found.");
                }

                _gameRepository.Delete(id);
                return Result.Ok();
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }

        public Result AddNewScore(long gameId, long userId, double score)
        {
            try
            {
                var game = _gameRepository.Get(gameId);
                if (game == null)
                {
                    return Result.Fail("Game not found.");
                }

                // Dodajemo novi rezultat za igrača
                var newScore = new GameScore(userId, score);
                game.Scores.Add(newScore);

                // Ažuriramo Highscore ako je potrebno
                if (score > game.Highscore)
                {
                    game.Highscore = score;
                }

                _gameRepository.Update(game);

                return Result.Ok();
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }
        public Result<string> AwardTopScorerCoupon()
        {
            try
            {
                // Fetch all games
                var games = _gameRepository.GetAll();

                // Define the date range for the last 7 days
                var endDate = DateTime.UtcNow;
                var startDate = endDate.AddDays(-7);

                var awardedPlayers = new List<string>(); // Keep track of players who received a coupon

                foreach (var game in games)
                {
                    // Skip games that have been checked recently
                    if (game.LastCheckedDate != null && (DateTime.UtcNow - game.LastCheckedDate.Value).TotalDays < 7)
                    {
                        continue;
                    }

                    // Find the top scorer for this game in the last 7 days
                    var topScore = game.Scores
                        .Where(score => score.AchievedAt >= startDate && score.AchievedAt <= endDate)
                        .OrderByDescending(score => score.Score)
                        .FirstOrDefault();

                    if (topScore == null)
                    {
                        continue; // No scores found for this game in the last 7 days
                    }

                    // Create a coupon for the top scorer
                    var coupon = new CouponDTO
                    {
                        Code = GenerateCouponCode(),
                        DiscountPercentage = 10, // Example: 10% discount
                        ExpiryDate = endDate.AddDays(30), // Valid for 30 days
                        TourId = null, // Applicable for all tours
                        AuthorId = topScore.PlayerId
                    };

                    _couponService.Create(coupon);

                    // Update the LastCheckedDate for the game
                    game.LastCheckedDate = DateTime.UtcNow;
                    _gameRepository.Update(game);

                    awardedPlayers.Add($"Player {topScore.PlayerId} for Game {game.Id}");
                }

                if (awardedPlayers.Count == 0)
                {
                    return Result.Fail("No scores found or all games have been recently checked.");
                }

                return Result.Ok($"Coupons awarded to: {string.Join(", ", awardedPlayers)}.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while awarding the coupons: {ex.Message}");
            }
        }

        private string GenerateCouponCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
