using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Text.Json.Serialization;

namespace Explorer.Blog.Core.Domain.Blogs
{
    public class Vote : ValueObject<Vote>
    {
        public int BlogId { get; private set; }
        public int UserId { get; private set; }
        public DateTime CreatedTime { get; private set; }
        public Markdown Mark { get; private set; }

        [JsonConstructor]
        public Vote(int userId, int blogId, Markdown mark)
        {
            UserId = userId;
            BlogId = blogId;
            Mark = mark;
            CreatedTime = DateTime.Now;
        }

        protected override bool EqualsCore(Vote other)
        {
            return UserId == other.UserId && BlogId == other.BlogId && Mark == other.Mark;
        }

        protected override int GetHashCodeCore()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + UserId.GetHashCode();
                hash = hash * 23 + BlogId.GetHashCode();
                hash = hash * 23 + Mark.GetHashCode();
                return hash;
            }
        }
    }

    public enum Markdown
    {
        Upvote,
        Downvote
    }
}
