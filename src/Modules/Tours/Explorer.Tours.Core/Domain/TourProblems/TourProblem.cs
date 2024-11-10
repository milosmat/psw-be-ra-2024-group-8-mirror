using Explorer.BuildingBlocks.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.TourProblems;

public class TourProblem : Entity
{
    public int UserId { get; private set; }
    public Tour Tour { get; private set; }
    public string Category { get; private set; }
    public string Priority { get; private set; }
    public string Description { get; private set; }
    public DateTime ReportedAt { get; private set; }
    public bool Resolved { get; private set; }
    public List<ProblemComment> ProblemComments { get; private set; }

    public TourProblem(int userId, Tour tour, string category, string priority, string description, DateTime reportedAt, bool resolved, List<ProblemComment> problemComments)
    {
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Invalid Description.");
        UserId = userId;
        Tour = tour;
        Category = category;
        Priority = priority;
        Description = description;
        ReportedAt = reportedAt;
        Resolved = resolved;
        ProblemComments = problemComments;
    }

    public Result AddComment(ProblemComment newC)
    {
        ProblemComments.Add(newC);
        return Result.Ok();
    }

}
