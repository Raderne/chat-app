using ChatMessagesApp.Core.Domain.Common;
using ChatMessagesApp.Core.Domain.Entities;

namespace ChatMessagesApp.Core.Domain.Events;

public class MessageSentEvent : DomainEvent
{
    public Message MessageCreated { get; }
    public string RecipientUserId { get; }

    public MessageSentEvent(Message message, string recipientUserId)
    {
        MessageCreated = message;
        RecipientUserId = recipientUserId;
        DateOccurred = DateTimeOffset.UtcNow;
    }
}
