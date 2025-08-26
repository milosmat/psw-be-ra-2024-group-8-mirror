using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Games.Core.Domain
{
    public class Game : Entity
    {
        public double Highscore { get; set; } = 0.0;
        public List<GameScore> Scores { get; set; } = new List<GameScore>();
        public DateTime? LastCheckedDate { get; set; }
        public Game()
        {
        }

        public Game(double highscore, List<GameScore> scores, DateTime? lastCheckedDate = null)
        {
            Highscore = highscore;
            Scores = scores;
            LastCheckedDate = lastCheckedDate;
        }
    }
}
