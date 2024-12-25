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

namespace Explorer.Blog.Tests.Integration
{

    [Collection("Sequential")]
    public class BlogsQueryTests : BaseBlogIntegrationTest
    {
        public BlogsQueryTests(BlogTestFactory factory) : base(factory) { }

        [Theory]
        [InlineData(0, 0, 3, 3)]
        [InlineData(1, 1, 1, 3)] // Testira stranicu sa 1 rezultat po stranici
        public void Retrieves_all(int pageNumber, int pageSize, int expectedResultsCount, int expectedTotalCount)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = ((ObjectResult)controller.GetAll(pageNumber, pageSize).Result)?.Value as PagedResult<BlogsDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(expectedResultsCount);
            result.TotalCount.ShouldBe(expectedTotalCount);
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