using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Explorer.Stakeholders.Core.Domain;

public class Notification :Entity
{
    public int SenderId { get; private set; }  // Ko je poslao
    public int FollowerId { get; private set; }  // Ko je primalac
    public bool IsRead { get; private set; }  // Da li je pročitana
    public long MessageId { get; private set; }  // Povezivanje sa porukom
    public Message Message { get; set; } // Navigacija na poruku

    public Notification(int senderId, int followerId, long messageId)
    {
        SenderId = senderId;
        FollowerId = followerId;
        IsRead = false;
        MessageId = messageId;
    }

    // Metoda za označavanje kao pročitano
    public void MarkAsRead()
    {
        IsRead = true;
    }

}
