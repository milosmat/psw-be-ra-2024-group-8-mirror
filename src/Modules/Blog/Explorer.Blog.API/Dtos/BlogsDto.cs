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
        public int UserId {  get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<string>? Images { get; set; }
        public BlogsStatus Status { get; set; }

        public List<VoteDto> Votes { get; set; } = new List<VoteDto>();
    }
    public enum BlogsStatus
    {
        Draft,
        Published,
        Closed
    }
}

