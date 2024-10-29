using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public
{
    public interface IProblemReplyService
    {
        Result<PagedResult<ProblemReplyDto>> GetPaged(int page, int pageSize);
        Result<ProblemReplyDto> Create(ProblemReplyDto problem);

        public Result<PagedResult<ProblemReplyDto>> GetByProblemId(int problemId, int page, int pageSize);
    }
}
