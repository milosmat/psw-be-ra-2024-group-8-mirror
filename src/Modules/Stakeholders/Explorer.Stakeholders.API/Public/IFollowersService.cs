using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public;

public interface IFollowersService
{
    Result<FollowersDto> Get(int id);
    Result<FollowersDto> Create(FollowersDto followersDto);
    Result<FollowersDto> Update(FollowersDto followersDto);
    Result Delete(int id);
    Result<PagedResult<FollowersDto>> GetPaged(int page, int pageSize);
    //Result<List<FollowersDto>>findByFollowerId(int followerId, int page, int pageSize);
    Result<PagedResult<FollowersDto>> GetPagedByFollowerId(int followerId, int page, int pageSize);
    Result<List<UserDto>> GetNonFollowedUsers(List<UserDto> allUsers, int currentUserId);

    Result<List<UserDto>> GetFollowedUsers(List<UserDto> allUsers, int currentUserId);

    Result DeleteByFollowerAndFollowingIds(int followerId, int followingId);


}
