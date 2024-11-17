using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain.TourProblems;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Explorer.Stakeholders.API.Dtos.TourProblemDto;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class TourProblemService : BaseService<TourProblemDto, TourProblem>, ITourProblemService
    {
        private readonly ITourProblemRepository _tourProblemRepository;
        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;

        public TourProblemService(ITourProblemRepository tourProblemRepository, IUserRepository userRepository, IMapper mapper) : base(mapper)
        {
            _userRepository = userRepository;
            _tourProblemRepository = tourProblemRepository;
            _mapper = mapper;
        }

        public Result<UserDto> GetUser(int userId)
        {
            var user = _userRepository.GetUser(userId);
            if (user == null)
            {
                return Result.Fail("User not found.");
            }
            var userDto = _mapper.Map<UserDto>(user);
            return Result.Ok(userDto);
        }

        public Result AddProblemComment(int problemId, ProblemCommentDto problemCommentDto)
        {
            var problemComment = _mapper.Map<ProblemComment>(problemCommentDto);
            var tourProblem = _tourProblemRepository.Get(problemId);
            if (tourProblem == null)
                return Result.Fail("Tour not found.");

            try
            {
                tourProblem.AddComment(problemComment);
                _tourProblemRepository.Update(tourProblem);
                return Result.Ok();
            }
            catch (Exception e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

        public Result<List<TourProblemDto>> GetAllForUser(long userId)
        {
            return MapToDto(_tourProblemRepository.GetAllForUser(userId));
        }

        public Result<TourProblemDto> Update(TourProblemDto touristEquipmentDto)
        {
            try
            {
                if (touristEquipmentDto == null)
                {
                    return Result.Fail(FailureCode.InvalidArgument).WithError("TourProblemDto cannot be null.");
                }
                var existingTouristEquipment = _tourProblemRepository.Get(touristEquipmentDto.Id);

                if (existingTouristEquipment == null)
                {
                    return Result.Fail(FailureCode.NotFound).WithError("TouristEquipment not found.");
                }
                _mapper.Map(touristEquipmentDto, existingTouristEquipment);
                var updatedTouristEquipment = _tourProblemRepository.Update(existingTouristEquipment);
                var updatedTouristEquipmentDto = _mapper.Map<TourProblemDto>(updatedTouristEquipment);

                return Result.Ok(updatedTouristEquipmentDto);
            }
            catch (Exception e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

        public Result<TourProblemDto> Create(TourProblemDto touristEquipmentDto)
        {
            try
            {
                if (touristEquipmentDto == null)
                {
                    return Result.Fail(FailureCode.InvalidArgument).WithError("TouristEquipmentDTO cannot be null.");
                }
                var touristEquipment = _mapper.Map<TourProblem>(touristEquipmentDto);
                var createdTouristEquipment = _tourProblemRepository.Create(touristEquipment);
                if (createdTouristEquipment == null)
                {
                    return Result.Fail(FailureCode.InvalidArgument).WithError("Creation failed.");
                }
                var createdTouristEquipmentDto = _mapper.Map<TourProblemDto>(createdTouristEquipment);
                return Result.Ok(createdTouristEquipmentDto);
            }
            catch (Exception e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

        /*public Result AddProblemComment(int problemId, TourProblemDto.ProblemCommentDto problemCommentDto)
        {
            throw new NotImplementedException();
        }*/
    }
}
