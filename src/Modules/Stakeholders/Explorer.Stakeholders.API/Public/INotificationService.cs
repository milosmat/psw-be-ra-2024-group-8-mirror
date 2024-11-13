using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public;

public interface INotificationService
{
    //Task<Result> SendMessageAndNotificationToFollowerAsync(int senderId, int followerId, string content, int? resourceId, ResourceType? resourceType);
    Task<Result> SendMessageAndNotificationToFollowerAsync(int senderId, int followerId, string content, string? resourceUrl, ResourceType? resourceType);

    // Metoda za dobijanje svih notifikacija za korisnika
    Task<Result<IEnumerable<NotificationDto>>> GetNotificationsForUserAsync(int followerId);
    Task<Result> MarkNotificationAsReadAsync(int notificationId);



}
