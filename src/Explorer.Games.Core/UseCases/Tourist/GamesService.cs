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

namespace Explorer.Games.Core.UseCases.Tourist
{
    public class GameService : IGameService
    {
        private readonly IGamesRepository _gameRepository;
        private readonly IMapper _mapper;

        public GameService(IGamesRepository gameRepository, IMapper mapper)
        {
            _gameRepository = gameRepository;
            _mapper = mapper;
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
    }
}
