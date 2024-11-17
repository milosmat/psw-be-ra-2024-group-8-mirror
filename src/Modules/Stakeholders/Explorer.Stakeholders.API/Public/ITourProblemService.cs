using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Explorer.Stakeholders.API.Dtos.TourProblemDto;

namespace Explorer.Stakeholders.API.Public
{
    public interface ITourProblemService
    {
        Result AddProblemComment(int problemId, ProblemCommentDto problemCommentDto);
        Result<List<TourProblemDto>> GetAllForUser(long userId);
        Result<TourProblemDto> Update(TourProblemDto touristEquipmentDto);
        Result<TourProblemDto> Create(TourProblemDto touristEquipmentDto);
        Result<UserDto> GetUser(int userId);
    }
}
