using Explorer.API.Controllers.Administrator.Administration;
using Explorer.API.Controllers.Author;
using Explorer.API.Controllers.Tourist;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Core.Domain.Blogs;
using Explorer.Blog.Core.UseCases;
using Explorer.Blog.Infrastructure.Database;
using Explorer.Tours.API.Public.Administration;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.Tests.Integration.Administration
{

    [Collection("Sequential")]
    public class CommentsCommandTests : BaseBlogIntegrationTest
    {
        public CommentsCommandTests(BlogTestFactory factory) : base(factory) { }

        [Theory]
        [InlineData(1, "Test comment", 1)] // User 1, Blog 1, Comment content
        public void Creates(int userId, string content, int blogId)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();
            var newComment = new CommentDto
            {
                Id = 0,
                UserId = userId,
                Text = content,
                BlogId = blogId,
                CreationTime = DateTime.UtcNow,
                LastModifiedTime = DateTime.UtcNow
            };

            // Act
            var result = ((ObjectResult)controller.Create(blogId, newComment).Result)?.Value as CommentDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.Text.ShouldBe(newComment.Text);
            result.UserId.ShouldBe(newComment.UserId);
            result.BlogId.ShouldBe(newComment.BlogId);
            result.CreationTime.ShouldBe(newComment.CreationTime);

            // Assert - Database
            var storedEntity = dbContext.Comments.FirstOrDefault(i => i.Id == result.Id);
            storedEntity.ShouldNotBeNull();
            storedEntity.Text.ShouldBe(newComment.Text);
        }

        [Theory]
        [InlineData(0, "Test comment", 400)] // Invalid userId returns 400 Bad Request
        [InlineData(-1, "Test comment", 400)] // Invalid userId returns 400 Bad Request
        public void Create_fails_invalid_data(int? userId, string content, int expectedStatusCode)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var newComment = new CommentDto
            {
                UserId = userId.GetValueOrDefault(),
                Text = content,
                BlogId = 1,
                CreationTime = DateTime.UtcNow
            };

            // Act
            var result = (ObjectResult)controller.Create(1, newComment).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(expectedStatusCode);
        }

        [Theory]
        [InlineData(2, "Updated comment content", 1, 1)]
        public void Updates(int id, string content, int blogId, int userId)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            var updatedComment = new CommentDto
            {
                Id = id,
                Text = content,
                BlogId = blogId,
                UserId = userId
            };

            // Act
            var result = ((ObjectResult)controller.Update(blogId, id, updatedComment).Result)?.Value as CommentDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(id);
            result.Text.ShouldBe(updatedComment.Text);

            // Assert - Database
            var storedEntity = dbContext.Comments.FirstOrDefault(i => i.Id == id);
            storedEntity.ShouldNotBeNull();
            storedEntity.Text.ShouldBe(content);
        }

        [Theory]
        [InlineData(-1000, "Updated comment", 404)] // Invalid comment ID
        public void Update_fails_invalid_id(int id, string content, int expectedStatusCode)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var updatedComment = new CommentDto
            {
                Id = id,
                Text = content,
                BlogId = 1
            };

            // Act
            var result = (ObjectResult)controller.Update(1, id, updatedComment).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(expectedStatusCode);
        }

        [Theory]
        [InlineData(1, true)]
        public void Deletes(int id, bool shouldExist)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            var preDeleteEntity = dbContext.Comments.FirstOrDefault(i => i.Id == id);
            if (shouldExist)
            {
                preDeleteEntity.ShouldNotBeNull();
            }
            else
            {
                preDeleteEntity.ShouldBeNull();
            }

            // Act: Call Delete method
            controller.Delete(1, id);

            // Assert - Check if entity was deleted from the database
            var postDeleteEntity = dbContext.Blogs.FirstOrDefault(i => i.Id == id);
            postDeleteEntity.ShouldBeNull(); // Should be null if successfully deleted or if it didn't exist initially


        }

        [Theory]
        [InlineData(-1000, 404)] // Invalid comment ID
        public void Delete_fails_invalid_id(int id, int expectedStatusCode)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = (ObjectResult)controller.Delete(1, id);

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(expectedStatusCode);
        }

        private static Explorer.API.Controllers.Author.CommentController CreateController(IServiceScope scope)
        {
            return new Explorer.API.Controllers.Author.CommentController(scope.ServiceProvider.GetRequiredService<ICommentService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
