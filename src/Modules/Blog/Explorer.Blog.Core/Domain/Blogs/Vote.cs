using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Explorer.Blog.Core.Domain.Blogs
{
    public class Vote : ValueObject<Vote>
    {
        public int UserId { get; private set; }
        public DateTime CreatedTime { get; private set; }
        public Markdown Mark { get; private set; }

        [JsonConstructor]
        public Vote(int userId, Markdown mark)
        { 
            UserId = userId;
            Mark = mark;
            CreatedTime = DateTime.Now;
        }   

        protected override bool EqualsCore(Vote other)
        {
            throw new NotImplementedException();
        }

        protected override int GetHashCodeCore()
        {
            throw new NotImplementedException();
        }
    }

    public enum Markdown
    {
        Upvote,
        Downvote
    }
}
