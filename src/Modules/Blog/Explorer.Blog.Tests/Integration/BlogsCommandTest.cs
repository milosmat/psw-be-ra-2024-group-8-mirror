using Explorer.Blog.API.Dtos;
using Explorer.Blog.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.API.Controllers.Author;
using Explorer.Blog.API.Public;
using Shouldly;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Explorer.Blog.Core.Domain.Blogs;
using Markdown = Explorer.Blog.Core.Domain.Blogs.Markdown;
using BlogsStatus = Explorer.Blog.API.Dtos.BlogsStatus;
using Explorer.Stakeholders.Core.Domain;
using static System.Net.Mime.MediaTypeNames;


namespace Explorer.Blog.Tests.Integration
{
    [Collection("Sequential")]
    public class BlogsCommandTests : BaseBlogIntegrationTest
    {
        public BlogsCommandTests(BlogTestFactory factory) : base(factory) { }


        [Theory]
        [InlineData(1, "Create", "Description", new[] { "image.png" }, BlogsStatus.Published)]
        [InlineData(1, "CreateForUpdate", "DescriptionForUpdate", new[] { "imageForUpdate.png" }, BlogsStatus.Published)]
        public void Creates(int userId, string title, string description, string[] images, BlogsStatus status)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            var newEntity = new BlogsDto
            {
                UserId = userId,
                Title = title,
                Description = description,
                CreatedDate = DateTime.UtcNow,
                Images = images.ToList(),
                Status = status,
                Votes = new List<VoteDto>() // Initialize as empty
            };


            // Act
            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as BlogsDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.Title.ShouldBe(newEntity.Title);
            result.Description.ShouldBe(newEntity.Description);
            result.CreatedDate.ShouldBe(newEntity.CreatedDate);
            result.Images.ShouldBe(newEntity.Images);
            result.Status.ShouldBe(newEntity.Status);
            result.Votes.ShouldBe(newEntity.Votes); // Check votes

            // Assert - Database
            var storedEntity = dbContext.Blogs.FirstOrDefault(i => i.Id == result.Id);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldBe(result.Id);
            //storedEntity.Votes.ShouldBe(newEntity.Votes); // Validate votes in DB
        }



        [Theory]
        [InlineData(null, "TestDescription", 500)]
        public void Create_fails_invalid_data(string title, string description, int expectedStatusCode)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var newEntity = new BlogsDto
            {
                Title = title,
                Description = description,
                Votes = new List<VoteDto>()
            };

            // Act
            var result = (ObjectResult)controller.Create(newEntity).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(expectedStatusCode);
        }


        [Theory]
        [InlineData(2, "UpdateTitle2", "UpdateDescription2")]
        public void Updates(int id, string title, string description)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            var updatedEntity = new BlogsDto
            {
                Id = id,
                Title = title,
                Description = description
            };

            // Act
            var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as BlogsDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(id);
            result.Title.ShouldBe(updatedEntity.Title);
            result.Description.ShouldBe(updatedEntity.Description);

            // Assert - Database
            var storedEntity = dbContext.Blogs.FirstOrDefault(i => i.Id == id);
            storedEntity.ShouldNotBeNull();
            storedEntity.Title.ShouldBe(title);
            storedEntity.Description.ShouldBe(description);
        }

        [Theory]
        [InlineData(-1000,"Test" ,"Description", 500)]
        public void Update_fails_invalid_id(int id, string title, string description, int expectedStatusCode)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new BlogsDto
            {
                Id = id,
                Title = title,
                Description = description
            };

            // Act
            var result = (ObjectResult)controller.Update(updatedEntity).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(expectedStatusCode);
        }


        [Theory]
        [InlineData("Create", "Description", new[] { "image.png" }, BlogsStatus.Published, true)] // Expected to exist and be deleted
        public void Deletes(string title, string description, string[] images, BlogsStatus status, bool shouldExist)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            var newBlog = new BlogsDto
            {
                UserId = 1,
                Title = "Initial title",
                Description = "Initial description",
                CreatedDate = DateTime.UtcNow,
                Images = images.ToList(),
                Status = status,
                Votes = new List<VoteDto>() // Initialize as empty
            };
            var createdBlog = ((ObjectResult)controller.Create(newBlog).Result)?.Value as BlogsDto;
            var createdBlogId = createdBlog?.Id ?? -1;

            // Pre-check: Verify the initial existence state based on shouldExist
            var preDeleteEntity = dbContext.Blogs.FirstOrDefault(b => b.Id == createdBlogId);
            if (shouldExist)
            {
                preDeleteEntity.ShouldNotBeNull();
            }
            else
            {
                preDeleteEntity.ShouldBeNull();
            }

            // Act: Call Delete method
            controller.Delete(createdBlogId);

            // Assert - Check if entity was deleted from the database
            var postDeleteEntity = dbContext.Blogs.FirstOrDefault(i => i.Id == createdBlogId);
            postDeleteEntity.ShouldBeNull(); // Should be null if successfully deleted or if it didn't exist initially
        }



        [Theory]
        [InlineData(-1000, 404)]
        public void Delete_fails_invalid_id(int id, int expectedStatusCode)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = (ObjectResult)controller.Delete(id);

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(expectedStatusCode);
        }

        [Theory]
        [InlineData(1, 1, Markdown.Upvote)] // Blog ID 1, User ID 1, Upvote should succeed
        public void AddVote_AddsUpvoteCorrectly(int blogId, int userId, Markdown mark)
        {
            // Arrange
            var blog = GetTestBlog(blogId);
            Vote vote = new Vote(userId, blogId, mark);


            // Act
            var result = blog.AddVote(vote);

            // Assert
            int countVote = CalculateVoteDifference(blog);
            blog.Votes.FirstOrDefault()?.Mark.ShouldBe(mark);
        }

        private int CalculateVoteDifference(Blogg blog)
        {
            var upvotes = blog.Votes.Count(v => v.Mark == Markdown.Upvote);
            var downvotes = blog.Votes.Count(v => v.Mark == Markdown.Downvote);
            return upvotes - downvotes;
        }

        private Blogg GetTestBlog(int blogId)
        {
            return new Blogg(blogId, 1, "Test Blog Title", "This is a test blog description.", new List<string>(), Core.Domain.Blogs.BlogsStatus.Published, new List<Vote>());
        }


        [Fact]
        public void CalculateTotalVotes_AfterMultipleVotes()
        {
            // Arrange
            var blog = GetTestBlog(1);
            blog.AddVote(new Vote(1, 1, Markdown.Upvote));
            blog.AddVote(new Vote(1, 2, Markdown.Upvote));
            blog.AddVote(new Vote(1, 3, Markdown.Downvote));

            // Act
            var totalVotes = CalculateVoteDifference(blog);

            // Assert
            totalVotes.ShouldBe(1); // 2 upvotes - 1 downvote
        }

        [Fact]
        public void AddVote_UpvoteAndDownvoteByDifferentUsers()
        {
            // Arrange
            var blog = GetTestBlog(1);
            blog.AddVote(new Vote(1, 1, Markdown.Upvote));
            blog.AddVote(new Vote(1, 2, Markdown.Downvote));

            // Act
            var totalVotes = CalculateVoteDifference(blog);

            // Assert
            totalVotes.ShouldBe(0); // 1 upvote - 1 downvote
            blog.Votes.Count.ShouldBe(2); // Two distinct votes
        }


        private static BlogsController CreateController(IServiceScope scope)
        {
            return new BlogsController(scope.ServiceProvider.GetRequiredService<IBlogsService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }

    }
}
