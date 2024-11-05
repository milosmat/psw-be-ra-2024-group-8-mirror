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


namespace Explorer.Blog.Tests.Integration
{
    [Collection("Sequential")]
    public class BlogsCommandTests : BaseBlogIntegrationTest
    {
        public BlogsCommandTests(BlogTestFactory factory) : base(factory) { }

        [Theory]
        [InlineData(1, "Create", "Description", new[] { "image.png" }, BlogsStatus.Published)]
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

            // Assert - Database
            var storedEntity = dbContext.Blogs.FirstOrDefault(i => i.Title == newEntity.Title);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldBe(result.Id);
        }


        [Theory]
        [InlineData(null, "Test", 400)]
        public void Create_fails_invalid_data(string title, string description, int expectedStatusCode)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var newEntity = new BlogsDto
            {
                Title = title,
                Description = description
            };

            // Act
            var result = (ObjectResult)controller.Create(newEntity).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(expectedStatusCode);
        }


        [Theory]
        [InlineData(-2, "UpdateTitle2", "UpdateDescription2")]
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
            var storedEntity = dbContext.Blogs.FirstOrDefault(i => i.Title == title);
            storedEntity.ShouldNotBeNull();
            storedEntity.Description.ShouldBe(description);
        }


        [Theory]
        [InlineData(-1000, "Test", 404)]
        public void Update_fails_invalid_id(int id, string title, int expectedStatusCode)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new BlogsDto
            {
                Id = id,
                Title = title
            };

            // Act
            var result = (ObjectResult)controller.Update(updatedEntity).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(expectedStatusCode);
        }


        [Theory]
        [InlineData(-3, 200)]
        public void Deletes(int id, int expectedStatusCode)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            // Act
            var result = (OkResult)controller.Delete(id);

            // Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(expectedStatusCode);

            // Assert - Database
            var storedEntity = dbContext.Blogs.FirstOrDefault(i => i.Id == id);
            storedEntity.ShouldBeNull();
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


        private static BlogsController CreateController(IServiceScope scope)
        {
            return new BlogsController(scope.ServiceProvider.GetRequiredService<IBlogsService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }

    }
}
