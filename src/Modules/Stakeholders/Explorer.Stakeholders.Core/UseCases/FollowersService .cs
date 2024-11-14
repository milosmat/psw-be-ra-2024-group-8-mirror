using AutoMapper;
using Azure;
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

namespace Explorer.Stakeholders.Core.UseCases;

public class FollowersService : CrudService<FollowersDto, Followers>, IFollowersService
{

    public FollowersService(ICrudRepository<Followers> repository, IMapper mapper) : base(repository, mapper)
    {
    }


    public Result<PagedResult<FollowersDto>> GetPagedByFollowerId(int followerId, int page, int pageSize)
    {
        var pagedResult = CrudRepository.GetPaged(page, pageSize);

        var mappedResult = MapToDto(pagedResult);

        if (mappedResult.IsFailed)
        {
            return Result.Fail(mappedResult.Errors.First().Message);
        }

        var filteredItems = mappedResult.Value.Results // Access the Results property
            .Where(f => f.followerId == followerId) // Filter by followerId
            .ToList();

        int totalFilteredCount = filteredItems.Count;

        var filteredPagedResult = new PagedResult<FollowersDto>(
            filteredItems, // The filtered items
            totalFilteredCount // Total count of filtered items
        );

        return Result.Ok(filteredPagedResult);
    }

    public Result<List<UserDto>> GetNonFollowedUsers(List<UserDto> allUsers, int currentUserId)
    {
        // Dobijemo sve korisnike koje korisnik prati
        var followedResult = GetPagedByFollowerId(currentUserId, 1, int.MaxValue);
        if (followedResult.IsFailed)
        {
            return Result.Fail(followedResult.Errors.First().Message);
        }

        // Kreiramo listu ID-eva korisnika koje korisnik prati
        var followedIds = followedResult.Value.Results.Select(f => f.followingId).ToList();

        // Filtriramo sve korisnike da zadržimo one koje korisnik NE prati
        var nonFollowedUsers = allUsers
            .Where(user => !followedIds.Contains((int)user.Id))  // Filter korisnika koje korisnik ne prati
            .ToList();

        return Result.Ok(nonFollowedUsers);
    }


    public Result<List<UserDto>> GetFollowedUsers(List<UserDto> allUsers, int currentUserId)
    {
        // Dobijemo sve korisnike koje korisnik prati
        var followedResult = GetPagedByFollowerId(currentUserId, 1, int.MaxValue);
        if (followedResult.IsFailed)
        {
            return Result.Fail(followedResult.Errors.First().Message);
        }

        // Kreiramo listu ID-eva korisnika koje korisnik prati
        var followedIds = followedResult.Value.Results.Select(f => f.followingId).ToList();

        // Filtriramo sve korisnike da zadržimo one koje korisnik prati
        var followedUsers = allUsers
            .Where(user => followedIds.Contains((int)user.Id))  // Filter korisnika koje korisnik prati
            .ToList();

        return Result.Ok(followedUsers);
    }

    public Result DeleteByFollowerId(int followerId)
    {
        // Pribavi sve zapise sa zadatim followerId koristeći postojeći metod
        var pagedResult = GetPagedByFollowerId(followerId, 1, int.MaxValue);

        if (pagedResult.IsFailed || pagedResult.Value == null || !pagedResult.Value.Results.Any())
        {
            return Result.Fail("No followers found for the given followerId.");
        }

        // Prolazi kroz rezultate i briši ih jedan po jedan
        foreach (var follower in pagedResult.Value.Results)
        {
            CrudRepository.Delete(follower.Id); // Brisanje po ID-u zapisa
        }

        return Result.Ok();
    }

    public Result DeleteByFollowingId(int followingId)
    {
        // Pribavi sve zapise gde se followingId poklapa sa zadatim using GetPagedByFollowerId
        var allFollowers = CrudRepository.GetPaged(1, int.MaxValue); // Preuzmi sve zapise
        var matchingFollowers = allFollowers.Results.Where(f => f.followingId == followingId).ToList();

        // Ako nema rezultata, vrati grešku
        if (!matchingFollowers.Any())
        {
            return Result.Fail("No followers found for the given followingId.");
        }

        // Brisanje svakog odgovarajućeg zapisa po ID-u
        foreach (var follower in matchingFollowers)
        {
            CrudRepository.Delete(follower.Id);
        }

        return Result.Ok();
    }

    public Result DeleteByFollowerAndFollowingIds(int followerId, int followingId)
    {
        // Pretražimo sve zapise sa zadatim `followerId` i `followingId`
        var allFollowers = CrudRepository.GetPaged(1, int.MaxValue); // Pribavi sve zapise
        var followerConnection = allFollowers.Results
            .FirstOrDefault(f => f.followerId == followerId && f.followingId == followingId);

        // Ako zapis ne postoji, vrati grešku
        if (followerConnection == null)
        {
            return Result.Fail("No follower connection found for the given followerId and followingId.");
        }

        // Ako postoji, brišemo zapis po ID-u
        CrudRepository.Delete(followerConnection.Id);

        return Result.Ok();
    }


}
