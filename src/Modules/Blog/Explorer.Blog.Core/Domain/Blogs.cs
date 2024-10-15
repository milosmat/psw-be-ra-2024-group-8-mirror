using Explorer.Blog.API.Dtos;
using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.Core.Domain
{
    public class Blogs : Entity
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public List<string>? Images { get; private set; }
        public BlogsStatus Status { get; private set; }

        public Blogs(string title, string description, DateTime createdDate, List<string>? images, BlogsStatus status)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Invalid Title.");
            if (description != null && string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description cannot be empty.");
            if (createdDate > DateTime.Now) throw new ArgumentException("Created date cannot be in the future.");

            Title = title;
            Description = description;
            CreatedDate = createdDate;
            Images = images ?? new List<string>();
            Status = status;
        }
    }
}
public enum BlogsStatus
{
    Draft,
    Published,
    Closed
}
