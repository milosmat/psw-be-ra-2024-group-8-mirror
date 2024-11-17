using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain;

public class Message : Entity
{
    public int SenderId { get; set; } // ID korisnika koji je poslao poruku
    public string Content { get; set; } // Sadržaj poruke
    public string? ResourceUrl { get; set; } // Opcioni ID resursa (tura ili blog)
    public ResourceType? ResourceType { get; set; } // Tip resursa, može biti Tura ili Blog

    public Message(int senderId, string content, string? resourceUrl, ResourceType? resourceType)
    {
        SenderId = senderId;
        Content = content;
        ResourceUrl = resourceUrl;
        ResourceType = resourceType;
    }
}

public enum ResourceType
{
    Tour,
    Blog
}

