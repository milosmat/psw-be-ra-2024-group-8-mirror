using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.TourProblems;

public class ProblemComment : Entity
{
    public string Text { get; private set; }
    public long UserId { get; private set; }
    public long TourProblemId { get; private set; }
    public DateTime CommentedAt { get; private set; }

    public ProblemComment(string text, long userId, long tourProblemId, DateTime commentedAt)
    {
        //if (userId <= 0) throw new ArgumentException("Invalid UserId.");
        if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Invalid Text.");
        Text = text;
        UserId = userId;
        TourProblemId = tourProblemId;
        CommentedAt = commentedAt;
    }
}
