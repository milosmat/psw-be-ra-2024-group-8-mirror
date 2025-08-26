using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos;
public class SendMessageRequest
{
    public int SenderId { get; set; }
    public int FollowerId { get; set; }
    public string Content { get; set; }
    public string? ResourceUrl { get; set; }
    public ResourceType? ResourceType { get; set; }
}

public class NotificationDto
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public int FollowerId { get; set; }
    public int MessageId { get; set; }
    public bool IsRead { get; set; }
    public MessageDto Message { get; set; }
}

public class MessageDto
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public int? OwnerId { get; set; }
    public string Content { get; set; }
    public string? ResourceUrl { get; set; }
    public ResourceType? ResourceType { get; set; }
}

public enum ResourceType
{
    Tour,
    Blog, 
    Club
}

