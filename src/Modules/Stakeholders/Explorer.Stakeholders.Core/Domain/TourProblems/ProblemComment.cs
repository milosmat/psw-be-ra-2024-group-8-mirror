using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.TourProblems;

public class ProblemComment : ValueObject<ProblemComment>
{
    public User User { get; private set; }
    public string Text { get; private set; }
    public DateTime CreatedAt { get; private set; }

    [JsonConstructor]
    public ProblemComment(User userId, string text, DateTime dateTime)
    {
        //if (userId <= 0) throw new ArgumentException("Invalid UserId.");
        if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Invalid Text.");
        User = userId;
        Text = text;
        CreatedAt = dateTime;
    }

    protected override bool EqualsCore(ProblemComment other)
    {
        throw new NotImplementedException();
    }

    protected override int GetHashCodeCore()
    {
        throw new NotImplementedException();
    }
}
