using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;

namespace Explorer.Stakeholders.API.Public
{
    public interface IProblemService
    {
        Result<PagedResult<ProblemDto>> GetPaged(int page, int pageSize);
        Result<ProblemDto> Update(ProblemDto problem);
        Result<ProblemDto> Create(ProblemDto problem);

        public Result<PagedResult<ProblemDto>> GetByTouristId(int touristId, int page, int pageSize);

        public Result<PagedResult<ProblemDto>> GetByTourId(int tourId, int page, int pageSize);
    }
}
