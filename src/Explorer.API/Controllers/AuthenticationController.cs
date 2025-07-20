using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Public.Tourist;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers;

[Route("api/users")]
public class AuthenticationController : BaseApiController
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IEmailService _emailService;
    private readonly IWalletService _walletService;

    public AuthenticationController(IAuthenticationService authenticationService, IEmailService emailService, IWalletService walletService)
    {
        _authenticationService = authenticationService;
        _emailService = emailService;
        _walletService = walletService;
    }

    [HttpPut]
    public ActionResult<AuthenticationTokensDto> RegisterTourist([FromBody] AccountRegistrationDto account)
    {
        var result = _authenticationService.Register(account);
        var wallet = _walletService.CreateWallet(_authenticationService.GetUserId(account.Username));
        return CreateResponse(result);
    }

    [HttpPost("login")]
    public ActionResult<AuthenticationTokensDto> Login([FromBody] CredentialsDto credentials)
    {
        var result = _authenticationService.Login(credentials);
        if(result.IsFailed)
        {
            var reasonWithStatusCode = result.Reasons.FirstOrDefault(r => r.Metadata.ContainsKey("code"));
            if(reasonWithStatusCode != null)
            {
                var statusCode = reasonWithStatusCode.Metadata["code"];
                if (statusCode.Equals(404))
                {
                    return NotFound(new { message = "Invalid username." });
                }
                return BadRequest(new { message = "Invalid password." });
            }
            return StatusCode(500, new {message = "Unexpected error occurred."});
        }
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult> Register([FromBody] AccountRegistrationDto account)
    {
        try
        {
            //samo ga prvo sacuvaj
            var result = _authenticationService.RegisterUser(account);
            if (result.IsFailed)
            {
                var reasonWithStatusCode = result.Reasons.FirstOrDefault(r => r.Metadata.ContainsKey("code"));
                if (reasonWithStatusCode != null)
                {
                    var statusCode = reasonWithStatusCode.Metadata["code"];

                    if (statusCode.Equals(409))
                    {
                        return Conflict(new { message = "Username already exists." });
                    }
                    else if (statusCode.Equals(400))
                    {
                        return BadRequest(new { message = result.Errors.Last().Message });
                    }
                }
                return StatusCode(500, new { message = "Unexpected error occurred." });


            }

            //sada posalji aktivacioni link
            string verificationLink = $"https://localhost:44333/api/users/verify?username={account.Username}";

            string body = $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset='utf-8'>
                        <title>Account Verification</title>
                    </head>
                    <body>
                        <p>Dear {account.Username},</p>
                        <p>Thank you for registering on GoTravel! Your account has been successfully created.</p>
                        <p>To activate your account and get started, please verify your email address by clicking the link below:</p>
                        <p><a href='{verificationLink}' target='_blank'>Click here to verify your email</a></p>
                        <p>Once your email is verified, you’ll be able to log in and enjoy all the adventures our platform offers.</p>
                        <p>Best regards,</p>
                        <p>The Group-8 Team</p>
                    </body>
                    </html>";


            var sendingResult = await _emailService.SendEmailToUserAsync(account.Email, "Verification", body);

            if (sendingResult.IsFailed)
            {
                return StatusCode(500, new { message = "Registration successful, but failed to send verification email." });
            }

            return Ok(new { message = "Registration successful. Please check your email for verification." });

        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }

    [HttpGet("verify")]
    public async Task<ActionResult<string>> VerifyUserAccount([FromQuery] string username)
    {
        try
        {
             UserDto existingUser = _authenticationService.GetByUsername(username);
            if (existingUser == null)
            {
                return NotFound("Unsuccessful activation - User not found with this username.");
            }
            if (existingUser.IsActive)
            {
                return Conflict("Your account is already verified.");
            }
            existingUser.IsActive = true;
            _authenticationService.UpdateUserStatus(existingUser.Id, true);

            var result = await _authenticationService.GenerateAndSendCouponToUserAsync(existingUser);
            if (result.IsFailed)
            {
                if (result.Errors.Any(e => e.Message.Equals("CouponGenerationFailed")))
                {
                    return StatusCode(500, "Coupon Generation Failed.");
                }
                return StatusCode(500, "Failed to send coupon code in email.");
            }
            _walletService.CreateWallet(_authenticationService.GetUserId(existingUser.Username));

            return Content("<p>Successful activation.</p><p> You can log in now. Click <a href='http://localhost:4200/home'>Log in</a></p>", 
                            "text/html");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }



}