using Microsoft.AspNetCore.Authorization;
using Explorer.Stakeholders.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.UseCases;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/allTourists")]
    public class UserTouristController : BaseApiController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IFollowersService _followersService;

        public UserTouristController(IAuthenticationService authenticationService, IFollowersService followersService)
        {
            _authenticationService = authenticationService;
            _followersService = followersService;
        }

        [HttpGet]
        public ActionResult<List<UserDto>> GetAllTourists()
        {
            var tourists = _authenticationService.GetAllTourists();
            return Ok(tourists); // Vraća 200 OK sa listom UserDto objekata
        }


        [HttpGet("nonFollowed/{currentUserId}")]
        public ActionResult<List<UserDto>> GetNonFollowedUsers(int currentUserId)
        {
            // Dobavljanje svih korisnika (turista) i filtriranje da se isključi trenutni korisnik
            var allUsers = _authenticationService.GetAllTourists()
                .Where(user => user.Id != currentUserId) // Izbaci samog sebe
                .ToList();

            // Dobijanje korisnika koje trenutni korisnik ne prati
            var result = _followersService.GetNonFollowedUsers(allUsers, currentUserId);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors.First().Message);
            }

            return Ok(result.Value); // Vraća 200 OK sa listom UserDto objekata koje korisnik ne prati
        }


        [HttpGet("followed/{currentUserId}")]
        public ActionResult<List<UserDto>> GetFollowedUsers(int currentUserId)
        {
            var allUsers = _authenticationService.GetAllTourists().ToList(); // Svi korisnici
            var result = _followersService.GetFollowedUsers(allUsers, currentUserId);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors.First().Message);
            }

            return Ok(result.Value); // Vraća 200 OK sa listom UserDto objekata koje korisnik prati
        }

    }
}
