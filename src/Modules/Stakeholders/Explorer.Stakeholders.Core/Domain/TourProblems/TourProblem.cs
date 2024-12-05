using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.TourProblems;

public class TourProblem : Entity
{
    public long TouristId { get; private set; }
    public int TourId { get; private set; }
    public long AuthorId { get; private set; }
    public string Category { get; private set; }
    public string Priority { get; private set; }
    public string Description { get; private set; }
    public DateTime ReportedAt { get; private set; }
    public bool Resolved { get; private set; }
    public List<ProblemComment> ProblemComments { get; private set; }
    public DateTime? ResolvingDue { get; private set; }
    public bool Closed { get; private set; }

    public TourProblem(long touristId, int tourId, long authorId, string category, string priority, string description, DateTime reportedAt, bool resolved, DateTime? resolvingDue, bool closed)
    {
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Invalid Description.");
        TouristId = touristId;
        AuthorId = authorId;
        TourId = tourId;
        Category = category;
        Priority = priority;
        Description = description;
        ReportedAt = reportedAt;
        Resolved = resolved;
        ProblemComments = new List<ProblemComment>();
        ResolvingDue = resolvingDue;
        Closed = closed;
    }

    public void AddComment(ProblemComment newC)
    {
        ProblemComments.Add(newC);
    }
}

