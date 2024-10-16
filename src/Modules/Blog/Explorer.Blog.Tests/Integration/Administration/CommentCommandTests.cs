using Explorer.API.Controllers.Administrator.Administration;
using Explorer.API.Controllers.Author;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Infrastructure.Database;
using Explorer.Tours.API.Public.Administration;
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
    public class CommentCommandTests : BaseBlogIntegrationTest
    {
        public CommentCommandTests(BlogTestFactory factory) : base(factory){ }

        [Fact]
        public void CreatesComment()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope); // Kreira CommentController instancu
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>(); // Pretpostavimo da koristiš BlogContext za bazu
            var newComment = new CommentDto
            {
                BlogId = 1, // Pretpostavi da blog sa ID 1 postoji u bazi
                UserId = 1, // Pretpostavi da korisnik sa ID 1 postoji u bazi
                CreationTime = DateTime.UtcNow,
                LastModifiedTime = DateTime.UtcNow,
                Text = "Ovo je testni komentar.",
                
            };

            // Act
            var result = ((ObjectResult)controller.Create(newComment).Result)?.Value as CommentDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.BlogId.ShouldBe(newComment.BlogId);
            result.UserId.ShouldBe(newComment.UserId);
            result.Text.ShouldBe(newComment.Text);

            // Assert - Database
            var storedComment = dbContext.Comments.FirstOrDefault(c => c.Text == newComment.Text);
            storedComment.ShouldNotBeNull();
            storedComment.Id.ShouldBe(result.Id);
            storedComment.BlogId.ShouldBe(result.BlogId);
            storedComment.UserId.ShouldBe(result.UserId);
            storedComment.Text.ShouldBe(result.Text);
        }

        [Fact]
        public void Create_fails_invalid_data()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var invalidComment = new CommentDto
            {
                Text = "", // Prazan tekst
                BlogId = 1,
                UserId = 1
            };

            // Act
            var result = (ObjectResult)controller.Create(invalidComment).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(400); // 400 Bad Request zbog nevalidnih podataka
        }

        [Fact]
        public void Updates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();
            var updatedComment = new CommentDto
            {
                Id = 1,
                Text = "Ažurirani komentar",
                BlogId = 1,
                UserId = 1
            };

            // Act
            var result = ((ObjectResult)controller.Update(updatedComment).Result)?.Value as CommentDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Text.ShouldBe(updatedComment.Text);

            // Assert - Database
            var storedComment = dbContext.Comments.FirstOrDefault(c => c.Text == "Ažurirani komentar");
            storedComment.ShouldNotBeNull();
            storedComment.Text.ShouldBe(updatedComment.Text);
        }

        [Fact]
        public void Update_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedComment = new CommentDto
            {
                Id = -1000, // Nevalidan ID
                Text = "Nevažeći komentar"
            };

            // Act
            var result = (ObjectResult)controller.Update(updatedComment).Result;

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(400); 
        }

        [Fact]
        public void Deletes()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            // Act
            var result = (OkResult)controller.Delete(-3);

            // Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200); // Uspešno obrisano

            // Assert - Database
            var storedComment = dbContext.Comments.FirstOrDefault(c => c.Id == -3);
            storedComment.ShouldBeNull(); // Proveri da li je komentar obrisan
        }

        [Fact]
        public void Delete_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = (ObjectResult)controller.Delete(-1000);

            // Assert
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404); // 404 Not Found za nevalidan ID
        }



        private static CommentController CreateController(IServiceScope scope)
        {
            return new CommentController(scope.ServiceProvider.GetRequiredService<ICommentService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }

    }
}
