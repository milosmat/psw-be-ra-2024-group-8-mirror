using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using FluentResults;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class ProblemService : CrudService<ProblemDto, Problem>, IProblemService
    {
        public ProblemService(ICrudRepository<Problem> repository, IMapper mapper) : base(repository, mapper) { }

        public Result<PagedResult<ProblemDto>> GetByTouristId(int touristId, int page, int pageSize)
        {
            try
            {
                var pagedResult = GetPaged(page, pageSize);
                if (pagedResult != null)
                {

                    var filteredTourProblems = pagedResult.Value.Results.Where(tp => tp.UserId == touristId).ToList();

                    var filteredTourProblemsPagedResult = new PagedResult<ProblemDto>(
                        filteredTourProblems,
                        filteredTourProblems.Count
                    );

                    return Result.Ok(filteredTourProblemsPagedResult);
                }
                return Result.Fail("pagedResult is null");
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }

        public Result<PagedResult<ProblemDto>> GetByTourId(int tourId, int page, int pageSize)
        {
            try
            {
                var pagedResult = GetPaged(page, pageSize);
                if (pagedResult != null)
                {

                    var filteredTourProblems = pagedResult.Value.Results.Where(tp => tp.TourId == tourId).ToList();

                    var filteredTourProblemsPagedResult = new PagedResult<ProblemDto>(
                        filteredTourProblems,
                        filteredTourProblems.Count
                    );

                    return Result.Ok(filteredTourProblemsPagedResult);
                }
                return Result.Fail("pagedResult is null");
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }
    }
}
