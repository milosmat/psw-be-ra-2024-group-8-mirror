using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.API.Dtos
{
    public class BlogsDto
    {
        public int Id { get; set; }
        public long UserId {  get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<string>? Images { get; set; }
        public BlogsStatus Status { get; set; }
        public Status BlogStatus { get; set; }


        public List<VoteDto> Votes { get; set; } = new List<VoteDto>();

        public List<CommentDto> Comments { get; set; } = new List<CommentDto>(); 
    }
    public enum BlogsStatus
    {
        Draft,
        Published,
        Closed
    }
    public enum Status
    {
        None,
        ReadOnly,
        Active,
        Famous
    }
}

