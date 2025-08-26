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
        public DateTime AchievedAt { get; set; } // New property for timestamp

        public GameScoreDTO(long playerId, double score, DateTime achievedAt)
        {
            PlayerId = playerId;
            Score = score;
            AchievedAt = achievedAt;
        }

        // Additional constructor for compatibility if timestamp is not provided
        public GameScoreDTO(long playerId, double score)
        {
            PlayerId = playerId;
            Score = score;
            AchievedAt = DateTime.UtcNow; // Default to current time
        }
    }
}
