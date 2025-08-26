using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Games.API.Dtos
{
    public class GameDTO
    {
        public long Id { get; set; }
        public double Highscore { get; set; } = 0.0;
        public List<GameScoreDTO> Scores { get; set; }
        public DateTime? LastCheckedDate { get; set; }
        public GameDTO()
        {
            Scores = new List<GameScoreDTO>();
        }
    }
}
