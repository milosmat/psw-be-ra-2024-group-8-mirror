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

        public Blogg(int id,int userId, string title, string description, List<string>? images, BlogsStatus status, List<Vote> votes)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Invalid Title.");
            if (description != null && string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description cannot be empty.");
            
            Id = id;
            UserId = userId;
            Title = title;
            Description = description;
            CreatedDate = DateTime.Now;
            Images = images ?? new List<string>();
            Status = status;
            Votes = votes ?? new List<Vote>();
            Comments = new List<Comment>();
        }
        public void AddComment(Comment newComment)
        {
            if (newComment == null) 
            {
                throw new ArgumentNullException(nameof(newComment));
            }
            newComment.SetBlogId(this.Id);
            Comments.Add(newComment);
        }
        public void RemoveComment(long commentId)
        {
            var comment = Comments.FirstOrDefault(c => c.Id == commentId);
            if(comment != null)
            {
                Comments.Remove(comment);
            }
            else
            {
                throw new KeyNotFoundException($"Comment with ID {commentId} not found");
            }
        }
        public void UpdateComment(Comment updatedComment)
        {
            var comment = Comments.FirstOrDefault(c => c.Id == updatedComment.Id);  
            if(comment == null)
            {
                throw new KeyNotFoundException($"Comment with ID {updatedComment.Id} not found");
            }
            else
            {
                comment.UpdateText(updatedComment.Text);
            }

        }
        public void DeleteComment(long commentId)
        {
            var comment = Comments.FirstOrDefault(c => c.Id == commentId);
            if(comment == null)
            {
                throw new KeyNotFoundException($"Comment with ID {commentId} not found");
            }
            else
            {
                Comments.RemoveAll(c=> c.Id == commentId);
            }
           
        }

        public Vote AddVote(Vote newVote)
        {
            var vote = Votes.Find(v => v.BlogId == newVote.BlogId && v.UserId == newVote.UserId);
            if(vote != null && vote.UserId==newVote.UserId && vote.BlogId==newVote.BlogId)
            {
                if((newVote.Mark == Markdown.Upvote && vote.Mark == Markdown.Upvote) || (newVote.Mark == Markdown.Downvote && vote.Mark == Markdown.Downvote))
                {
                    Votes.RemoveAll(v => v.UserId == vote.UserId && v.BlogId == vote.BlogId);
                }
                if((newVote.Mark == Markdown.Upvote && vote.Mark == Markdown.Downvote) || (newVote.Mark == Markdown.Downvote && vote.Mark == Markdown.Upvote))
                {
                    Votes.RemoveAll(v => v.UserId == vote.UserId && v.BlogId == vote.BlogId);
                    Votes.Add(newVote);
                }
            }
            else
            {
                Votes.Add(newVote);
            }
            return newVote;

        }

    }

    public enum BlogsStatus
    {
        Draft,
        Published,
        Closed
    }

     
}
