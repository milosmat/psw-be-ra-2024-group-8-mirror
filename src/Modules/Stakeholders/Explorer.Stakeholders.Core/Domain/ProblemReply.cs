using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain
{
    public class ProblemReply : Entity
    {
        public int UserId { get; private set; }
        public int ProblemId { get; private set; }
        public DateTime CreationTime { get; private set; }
        public string? Text { get; private set; }

        public ProblemReply(int userId, int problemId, string text)
        {
            UserId = userId;
            ProblemId = problemId;
            CreationTime = DateTime.Now;
            Text = text;
        }
    }
}
