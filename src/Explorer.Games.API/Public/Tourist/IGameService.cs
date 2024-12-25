using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Games.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Games.API.Public.Tourist
{

    public interface IGameService
    {
        // Metoda za paginaciju rezultata igara
        Result<PagedResult<GameDTO>> GetPaged(int page, int pageSize);

        // Metoda za kreiranje nove igre
        Result<GameDTO> Create(GameDTO game);

        // Metoda za ažuriranje postojećeg zapisa o igri
        Result<GameDTO> Update(GameDTO game);

        // Metoda za dohvat igre prema ID-u
        Result<GameDTO> Get(long id);

        // Metoda za brisanje igre prema ID-u
        Result Delete(long id);

        Result AddNewScore(long gameId, long userId, double score);
        Result<string> AwardTopScorerCoupon();
    }
}
