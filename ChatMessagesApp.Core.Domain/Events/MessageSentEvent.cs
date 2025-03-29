using ChatMessagesApp.Core.Domain.Common;
using ChatMessagesApp.Core.Domain.Entities;

namespace ChatMessagesApp.Core.Domain.Events;

public class MessageSentEvent : DomainEvent
{
    public Message MessageCreated { get; }
    public List<string> ParticipantIds { get; }

    public MessageSentEvent(Message message, List<string> participantIds)
    {
        MessageCreated = message;
        ParticipantIds = participantIds;
        DateOccurred = DateTimeOffset.UtcNow;
    }
}
