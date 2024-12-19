using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Games.Core.Domain
{
    public class GameScore : Entity

    {
        public long PlayerId { get; set; }
        public double Score { get; set; }

        public GameScore() { }
            
        public GameScore(long playerId, double score)
        {
            PlayerId = playerId;
            Score = score;
        }
    }
}
