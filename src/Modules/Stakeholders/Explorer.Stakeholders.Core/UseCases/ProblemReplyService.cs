using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class ProblemReplyService : CrudService<ProblemReplyDto, ProblemReply>, IProblemReplyService
    {
        public ProblemReplyService(ICrudRepository<ProblemReply> repository, IMapper mapper) : base(repository, mapper) { }

        public Result<PagedResult<ProblemReplyDto>> GetByProblemId(int problemId, int page, int pageSize)
        {
            try
            {
                var pagedResult = GetPaged(page, pageSize);
                if (pagedResult != null)
                {

                    var filteredTourProblems = pagedResult.Value.Results.Where(tp => tp.ProblemId == problemId).ToList();

                    var filteredTourProblemsPagedResult = new PagedResult<ProblemReplyDto>(
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
