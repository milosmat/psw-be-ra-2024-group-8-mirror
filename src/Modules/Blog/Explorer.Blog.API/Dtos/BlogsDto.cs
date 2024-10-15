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
        public string Description { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public List<string>? Images { get; private set; }
        public BlogsStatus Status { get; private set; }
    }
    public enum BlogsStatus
    {
        Draft,
        Published,
        Closed
    }
}
