using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Games.API.Dtos;
using Explorer.Games.Core.Domain.RepositoryInterfaces;
using Explorer.Games.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using Explorer.Games.API.Public.Tourist;
using Explorer.Payments.API.Public.Tourist;
using Explorer.Payments.API.Dtos;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Encounters.API.Public.Tourist;

namespace Explorer.Games.Core.UseCases.Tourist
{
    public class GameService : IGameService
    {
        private readonly IGamesRepository _gameRepository;
        private readonly IMapper _mapper;
        private readonly ICouponService _couponService;
        private readonly ITouristProfileService _touristProfileService;
        private readonly IUserRepository _userRepository;
        private readonly IGameScoreRepository _gameScoreRepository;
        public GameService(IGamesRepository gameRepository, IMapper mapper, ICouponService couponService, IUserRepository userRepository, IGameScoreRepository gameScoreRepository, ITouristProfileService touristProfileService)
        {
            _gameRepository = gameRepository;
            _mapper = mapper;
            _couponService = couponService;
            _userRepository = userRepository;
            _gameScoreRepository = gameScoreRepository;
            _touristProfileService = touristProfileService;
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
        public Result AwardTopScorerCoupon()
        {
            try
            {
                // Fetch all games
                var games = _gameRepository.GetAll();
                var gamesScores = _gameScoreRepository;
                // Define the date range for the last 7 days
                var endDate = DateTime.UtcNow;
                var startDate = endDate.AddDays(-7);


                foreach (var game in games)
                {
                    // Skip games that have been checked recently
                    if (game.LastCheckedDate != null && (DateTime.UtcNow - game.LastCheckedDate.Value).TotalDays < 7)
                    {
                        continue;
                    }

                    // Find the top scorer for this game in the last 7 days
                    // Query GameScore repository for the highest score in the last 7 days
                    var topScore = _gameScoreRepository
                        .GetAll() // Fetch all scores
                        .Where(gameScore => gameScore.AchievedAt >= startDate && gameScore.AchievedAt <= endDate)
                        .OrderByDescending(score => score.Score)
                        .FirstOrDefault();

                    if (topScore == null || topScore.PlayerId <= 0)
                    {
                        continue; // Skip if no scores found or the PlayerId is invalid
                    }

                    // Validate the player exists before creating a coupon
                    var playerExists = _userRepository.GetUser(topScore.PlayerId);
                    if (playerExists == null)
                    {
                        continue; // Skip if the player does not exist
                    }
                    var tourist = _touristProfileService.GetTouristByUsername(playerExists.Username);

                    // Create a coupon for the top scorer
                    var coupon = new CouponDTO
                    {
                        Code = GenerateCouponCode(),
                        DiscountPercentage = 10, // Example: 10% discount
                        ExpiryDate = endDate.AddDays(30), // Valid for 30 days
                        TourId = null, // Applicable for all tours
                        AuthorId = topScore.PlayerId
                    };

                    var createdCoupon = _couponService.Create(coupon);
                    if (createdCoupon != null)
                    {
                        _touristProfileService.AddCouponToTourist(tourist.Value.Id, createdCoupon.Id);
                    }
                    // Update the LastCheckedDate for the game
                    game.LastCheckedDate = DateTime.UtcNow;
                    _gameRepository.Update(game);

                }

                return Result.Ok();
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
