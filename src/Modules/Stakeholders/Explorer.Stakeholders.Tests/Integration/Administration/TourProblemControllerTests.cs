using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Explorer.Blog.API.Dtos;
using Explorer.API.Controllers;
using Explorer.Tours.API.Public.Author;
using static Explorer.Stakeholders.API.Dtos.TourProblemDto;
using EmptyFiles;
using Explorer.Stakeholders.Core.Domain.TourProblems;
using Explorer.Tours.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Tests.Integration.Administration
{
    public class TourProblemControllerTests : BaseStakeholdersIntegrationTest
    {
        public TourProblemControllerTests(StakeholdersTestFactory factory) : base(factory) { }

        [Theory]
        [InlineData(1, 101, 2001, "Category1", "High", "Description1", false, null, false)]
        [InlineData(2, 102, 2002, "Category2", "Medium", "aa", true, null, true)]
        public void CreatesTourProblem(long touristId, int tourId, long authorId, string category, string priority, string description, bool resolved, string resolvingDueStr, bool closed)
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            var resolvingDue = string.IsNullOrEmpty(resolvingDueStr) ? (DateTime?)null : DateTime.Parse(resolvingDueStr);

            var newTourProblem = new TourProblemDto
            {
                TouristId = touristId,
                TourId = tourId,
                AuthorId = authorId,
                Category = category,
                Priority = priority,
                Description = description,
                Resolved = resolved,
                ResolvingDue = resolvingDue,
                Closed = closed
            };

            var result = ((ObjectResult)controller.Create(newTourProblem).Result)?.Value as TourProblemDto;

            result.ShouldNotBeNull();
            result.TouristId.ShouldBe(touristId);
            result.Description.ShouldBe(description);

            var storedProblem = dbContext.TourProblems.FirstOrDefault(tp => tp.Id == result.Id);
            storedProblem.ShouldNotBeNull();
            storedProblem.Description.ShouldBe(description);
            storedProblem.Category.ShouldBe(category);
        }

        [Theory]
        [InlineData(10, "UpdatedCategory", "Low", "UpdatedDescription")]
        public void UpdatesTourProblem(long id, string category, string priority, string description)
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            var existingTourProblem = dbContext.TourProblems.FirstOrDefault(tp => tp.Id == id);
            if (existingTourProblem == null)
            {
                existingTourProblem = new TourProblem(
                    1,  // TouristId
                    2,  // AuthorId
                    3,  // TourId
                    "OriginalCategory",
                    "High",
                    "OriginalDescription",
                    DateTime.Now.ToUniversalTime(),
                    false,
                    DateTime.Now.ToUniversalTime(),
                    false
                );
                dbContext.TourProblems.Add(existingTourProblem);
                dbContext.SaveChanges();
                id = existingTourProblem.Id; // 🔑 OVDE uzmeš pravi ID iz baze
            }

            var updatedTourProblem = new TourProblemDto
            {
                Id = (int)id,
                TouristId = 1,
                AuthorId = 2,
                TourId = 3,
                Category = category,
                Priority = priority,
                Description = description,
                ReportedAt = DateTime.Now.ToUniversalTime(),
                Resolved = true,
                ProblemComments = null,
                ResolvingDue = null,
                Closed = false
            };

            var result = ((ObjectResult)controller.Update(updatedTourProblem).Result)?.Value as TourProblemDto;

            /*var actionResult = controller.Update(updatedTourProblem).Result;
            var result = ((ObjectResult)actionResult)?.Value as TourProblemDto;*/

            Console.WriteLine(result);

            result.ShouldNotBeNull();
            //result.Id.ShouldBe((int)id);
            result.Category.ShouldBe(category);

            var storedProblem = dbContext.TourProblems.FirstOrDefault(tp => tp.Id == updatedTourProblem.Id);
            storedProblem.ShouldNotBeNull();
            storedProblem.Category.ShouldBe(category);
        }


        [Theory]
        [InlineData(-1, "Comment text")]
        public void AddsCommentToTourProblem(long problemId, string commentText)
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            var problem = new TourProblem(
                1, // TouristId
                2, // AuthorId
                3, // TourId
                "SomeCategory",
                "Low",
                "SomeDescription",
                DateTime.Now.ToUniversalTime(),
                false,
                DateTime.Now.ToUniversalTime(),
                false
            );
            dbContext.TourProblems.Add(problem);
            dbContext.SaveChanges();

            var commentDto = new ProblemCommentDto
            {
                Text = commentText,
                UserId = 1,
                TourProblemId = problem.Id, // 🔑 pravi ID
                CommentedAt = DateTime.Now.ToUniversalTime()
            };

            controller.AddProblemComment((int)problem.Id, commentDto);

            var storedProblem = dbContext.TourProblems
                .Include(tp => tp.ProblemComments)
                .FirstOrDefault(tp => tp.Id == problem.Id);

            storedProblem.ShouldNotBeNull();
            storedProblem.ProblemComments.ShouldContain(c => c.Text == commentText);

        }

        private static TourProblemController CreateController(IServiceScope scope)
        {
            return new TourProblemController(
                scope.ServiceProvider.GetRequiredService<ITourProblemService>(),
                scope.ServiceProvider.GetRequiredService<ITourService>()
            )
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}