using Explorer.Blog.API.Dtos;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

//[Authorize(Policy = "touristPolicy")]
[Route("api/notifications")]
public class NotificationController : BaseApiController
{
    private readonly INotificationService _notificationService;
    private readonly IMessageService _messageService;

    public NotificationController(INotificationService notificationService, IMessageService messageService)
    {
        _notificationService = notificationService;
        _messageService = messageService;
    }


    [HttpPost("send")]
    public async Task<IActionResult> SendMessageToFollower([FromBody] SendMessageRequest request)
    {
        if (request.ResourceType == ResourceType.Club)
        {
            // Kreiranje MessageDto objekta
            MessageDto newMessage = new MessageDto
            {
                ResourceType = request.ResourceType,
                SenderId = request.SenderId,
                OwnerId = request.FollowerId,
                Content = request.Content,
                ResourceUrl = request.ResourceUrl
            };

            // Poziv metode za kreiranje poruke specifične za Club
            try
            {
                MessageDto messageResult = _messageService.Create(request.FollowerId, newMessage);

                // Ako je poruka uspešno kreirana, vraćamo uspešan odgovor
                return Ok(new { Message = "Message created successfully for Club.", Data = messageResult });
            }
            catch (Exception ex)
            {
                // Ako se dogodi greška prilikom kreiranja poruke
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Nastavak za ostale ResourceType vrednosti (asinhrono)
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