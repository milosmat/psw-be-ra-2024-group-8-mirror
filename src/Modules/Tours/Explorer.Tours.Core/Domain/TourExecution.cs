using Explorer.BuildingBlocks.Core.Domain;
namespace Explorer.Tours.Core.Domain;
public class TourExecution : Entity
{
    public int TourId { get; private set; }
    public int UserId { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }
    public DateTime? LastActivity { get; private set; }
    public TourExecutionStatus Status { get; private set; } // IN_PROGRESS, COMPLETED, ABANDONED
    public List<VisitedCheckpoint> visitedCheckpoints { get; private set; }

    public TourExecution(int tourId, int userId)
    {
        TourId = tourId;
        UserId = userId;
        StartTime = DateTime.UtcNow;
        LastActivity = DateTime.UtcNow;
        Status = TourExecutionStatus.IN_PROGRESS;
        visitedCheckpoints = new List<VisitedCheckpoint>();
    }

    public void CompleteTour()
    {
        if (Status == TourExecutionStatus.IN_PROGRESS)
        {
            Status = TourExecutionStatus.COMPLETED;
            EndTime = DateTime.UtcNow;
            LastActivity = EndTime;
        }
    }

    public void VisitCheckpoint(VisitedCheckpoint checkpoint)
    {
        visitedCheckpoints.Add(checkpoint);
    }

    public void AbandonTour()
    {
        if (Status == TourExecutionStatus.IN_PROGRESS)
        {
            Status = TourExecutionStatus.ABANDONED;
            EndTime = DateTime.UtcNow;
            LastActivity = EndTime;
        }
    }

}

public enum TourExecutionStatus
{
    IN_PROGRESS, ABANDONED, COMPLETED
}
