using Explorer.API.Controllers.Author;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
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
    public class CommentQueryTests : BaseBlogIntegrationTest
    {
        public CommentQueryTests(BlogTestFactory factory) : base(factory) { }

       
        [Theory]
        [InlineData(0, 3, 3)]  // Prva stranica sa 3 komentara
        public void Retrieves_all(int pageNumber, int pageSize, int expectedResultsCount)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = ((ObjectResult)controller.GetAll(-1, pageNumber, pageSize).Result)?.Value as PagedResult<CommentDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(expectedResultsCount);
            result.TotalCount.ShouldBeGreaterThanOrEqualTo(expectedResultsCount);
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
