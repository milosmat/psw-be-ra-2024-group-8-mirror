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
        [InlineData(-11, "Test comment", -1)] // User 1, Blog 1, Comment content
        public void Creates(long userId, string content, long blogId)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            var newComment = new CommentDto
            {
                UserId = userId,
                Text = content,
                BlogId = blogId,
                CreationTime = DateTime.UtcNow,
                LastModifiedTime = DateTime.UtcNow
            };

            // Act
            var result = ((ObjectResult)controller.Create(blogId, newComment).Result)?.Value as CommentDto;
            var createdCommentId = result?.Id ?? 0;
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
        [InlineData(null, "Test comment", 404)] //Null userId
        [InlineData(1L, "", 404)] // Empty comment text
        public void Create_fails_invalid_data(long? userId, string content, int expectedStatusCode)
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
        [InlineData("Updated comment content", -1L, 1L)]
        public void Updates(string content, long blogId, long userId)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            var newComment = new CommentDto
            {
                UserId = userId,
                Text = "Initial comment content",
                BlogId = blogId,
                CreationTime = DateTime.UtcNow,
                LastModifiedTime = DateTime.UtcNow,
            };

            var createdComment = ((ObjectResult)controller.Create(blogId, newComment).Result)?.Value as CommentDto;
            var createdCommentId = createdComment?.Id ?? -1;


            var updatedComment = new CommentDto
            {
                Id = createdCommentId,
                Text = content,
                BlogId = blogId,
                UserId = userId,
                CreationTime = createdComment.CreationTime,
                LastModifiedTime = DateTime.UtcNow
            };

            // Act
            var result = ((ObjectResult)controller.Update(blogId, createdCommentId, updatedComment).Result)?.Value as CommentDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(createdCommentId);
            result.Text.ShouldBe(updatedComment.Text);

            // Assert - Database
            var storedEntity = dbContext.Comments.FirstOrDefault(i => i.Id == createdCommentId);
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
        [InlineData(-1L, 1L, "", 404)] //Empty text comment
        public void Update_fails_invalid_content(long blogId, long userId, string content, int expectedStatusCode)
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            var newComment = new CommentDto
            {
                UserId = userId,
                BlogId = blogId,
                Text = "Initial text",
                CreationTime = DateTime.UtcNow,
                LastModifiedTime = DateTime.UtcNow,
            };

            var createdComment = ((ObjectResult)controller.Create(blogId, newComment).Result)?.Value as CommentDto;
            var createdCommentId = createdComment?.Id ?? -1;

            var updatedComment = new CommentDto
            {
                Id = createdCommentId,
                UserId = userId,
                BlogId = blogId,
                CreationTime = createdComment.CreationTime,
                LastModifiedTime = DateTime.UtcNow,
                Text = content
            };

            //Act
            var result = (ObjectResult)controller.Update(1, updatedComment.Id, updatedComment).Result;

            //Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(expectedStatusCode);

        }

        [Theory]
        [InlineData(true)]
        public void Deletes(bool shouldExist)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            var comment = new CommentDto
            {
                UserId = 1,
                Text = "Test comment for delete.",
                CreationTime = DateTime.UtcNow,
                LastModifiedTime = DateTime.UtcNow,
                BlogId = -1
            };
            var createdComment = ((ObjectResult)controller.Create(-1, comment).Result)?.Value as CommentDto;
            var createdCommentId = createdComment?.Id ?? -1;

            var preDeleteEntity = dbContext.Comments.FirstOrDefault(c => c.Id == createdCommentId);
            if (shouldExist)
            {
                preDeleteEntity.ShouldNotBeNull();
            }
            else
            {
                preDeleteEntity.ShouldBeNull();
            }



            controller.Delete(-1, createdCommentId);


            // Assert - Check if entity was deleted from the database
            var postDeleteEntity = dbContext.Comments.FirstOrDefault(i => i.Id == createdCommentId);
            postDeleteEntity.ShouldBeNull(); // Should be null if successfully deleted or if it didn't exist initially


        }

        [Theory]
        [InlineData(-1000L, 404)] // Invalid comment ID
        public void Delete_fails_invalid_id(long id, int expectedStatusCode)
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
