using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System.Collections.Generic;
using Explorer.Stakeholders.API.Public;
using ResourceType = Explorer.Stakeholders.API.Dtos.ResourceType;
using AutoMapper;
using Azure;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public NotificationService(
            INotificationRepository notificationRepository,
            IMessageRepository messageRepository,
            IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;
        }


        public async Task<Result> SendMessageAndNotificationToFollowerAsync(int senderId, int followerId, string content, string? resourceUrl, ResourceType? resourceType)
        {

            // Provera dužine sadržaja poruke
            if (content.Length > 280)
            {
                return Result.Fail("Message content exceeds 280 characters.");
            }

            // Ako je resourceType "Club", preskačemo kreiranje notifikacije
            if (resourceType != ResourceType.Club)
            {
                // Kreiranje poruke
                var message = new Message(senderId, content, resourceUrl, (Domain.ResourceType?)resourceType, followerId);
                _messageRepository.Create(message); // Spremamo poruku u bazu

           
                // Kreiranje notifikacije samo ako resourceType nije "Club"
                var notification = new Notification(senderId, followerId, message.Id);
                _notificationRepository.Create(notification); // Spremamo notifikaciju u bazu
            }

            return Result.Ok();
        }



        public async Task<Result> MarkNotificationAsReadAsync(int notificationId)
        {
            var notification =  _notificationRepository.Get(notificationId);

            if (notification == null)
            {
                return Result.Fail("Notification not found.");
            }

            notification.MarkAsRead();

            _notificationRepository.Update(notification);

            return Result.Ok();
        }


        public async Task<Result<IEnumerable<NotificationDto>>> GetNotificationsForUserAsync(int followerId)
        {
            var notifications = await _notificationRepository.GetNotificationsByUserIdAsync(followerId);


            var readNotifications = notifications.Where(n => !n.IsRead);


            // Mapiramo entitete notifikacija u DTO objekte
            var notificationDtos = readNotifications.Select(n => new NotificationDto
            {
                Id = (int)n.Id,
                SenderId = n.SenderId,
                FollowerId = n.FollowerId,
                MessageId = (int)n.MessageId,
                IsRead = n.IsRead,
                Message = new MessageDto
                {
                    Id = (int)n.Message.Id,
                    SenderId = n.Message.SenderId,
                    Content = n.Message.Content,
                    ResourceUrl = n.Message.ResourceUrl,
                    ResourceType = (ResourceType?)n.Message.ResourceType
                }
            });

            return Result.Ok(notificationDtos); 
        }
    }
}

