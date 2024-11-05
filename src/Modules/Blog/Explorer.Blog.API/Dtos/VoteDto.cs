using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.API.Dtos
{
    public class VoteDto
    {
        public int BlogId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedTime { get; set; }
        public Markdown Mark { get; set; }
    }
    public enum Markdown
    {
        Upvote,
        Downvote
    }
}
