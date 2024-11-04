using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Explorer.Blog.Core.Domain.Blogs
{
    public class Blogg: Entity
    {
        public int UserId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public List<string>? Images { get; private set; }
        public BlogsStatus Status { get; private set; }
        public List<Vote> Votes { get; private set; }
        public List<Comment> Comments { get; private set; }

        public Blogg() { }

        public Blogg(int userId, string title, string description, List<string>? images, BlogsStatus status)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Invalid Title.");
            if (description != null && string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description cannot be empty.");
            
            UserId = userId;
            Title = title;
            Description = description;
            CreatedDate = DateTime.Now;
            Images = images ?? new List<string>();
            Status = status;
        }

    }

    public enum BlogsStatus
    {
        Draft,
        Published,
        Closed
    }
     
}
