using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Games.API.Dtos
{
    public class GameScoreDTO
    {
        public long PlayerId { get; set; }
        public double Score { get; set; }

        public GameScoreDTO(long playerId, double score)
        {
            PlayerId = playerId;
            Score = score;
        }
    }
}
