using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Blog.Core.Domain.Blogs
{
    public class Comment : Entity
    {
        public int BlogId { get; private set; }
        public int UserId { get; private set; }
        public DateTime CreationTime { get; private set; }
        public DateTime LastModifiedTime { get; private set; }
        public string Text { get; private set; }

        public Comment(int blogId, int userId, string text)
        {
            if(blogId<=0) throw new ArgumentException("Blog ID must be positive.", nameof(blogId));
            if (userId <= 0) throw new ArgumentException("User ID must be positive.", nameof(userId));
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Comment text cannot be empty.", nameof(text));

            BlogId = blogId;
            UserId = userId;
            CreationTime = DateTime.Now;
            LastModifiedTime = CreationTime;
            Text = text;
        }

    }
}
