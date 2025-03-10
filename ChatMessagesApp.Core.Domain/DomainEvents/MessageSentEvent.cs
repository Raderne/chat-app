using ChatMessagesApp.Core.Domain.Common;
using ChatMessagesApp.Core.Domain.Entities;

namespace ChatMessagesApp.Core.Domain.DomainEvents;

public class MessageSentEvent : DomainEvent
{
    public Message Message { get; }
    public Guid ConversationId { get; }

    public MessageSentEvent(Message message, Guid conversationId)
    {
        Message = message;
        ConversationId = conversationId;
    }
}
