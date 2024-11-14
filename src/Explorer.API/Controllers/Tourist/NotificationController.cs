using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/notifications")]
public class NotificationController : BaseApiController
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }


    [HttpPost("send")]
    public async Task<IActionResult> SendMessageToFollower([FromBody] SendMessageRequest request)
    {
        var result = await _notificationService.SendMessageAndNotificationToFollowerAsync(
            request.SenderId,
            request.FollowerId,
            request.Content,
            request.ResourceUrl,
            request.ResourceType);

        if (result.IsSuccess)
        {
            return Ok(new { Message = "Message sent and notification created successfully." });
        }

        return BadRequest(result.Errors);
    }


    [HttpPut("mark-as-read/{notificationId}")]
    public async Task<IActionResult> MarkAsRead(int notificationId)
    {
        var result = await _notificationService.MarkNotificationAsReadAsync(notificationId);

        if (result.IsSuccess)
        {
            return Ok(new { Message = "Notification marked as read." });
        }

        return BadRequest(result.Errors);
    }



    [HttpGet("{userId}")]
    public async Task<IActionResult> GetNotificationsForUser(int userId)
    {
        var result = await _notificationService.GetNotificationsForUserAsync(userId);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return NotFound(new { Message = "Notifications not found." });
    }


}