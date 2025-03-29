using ChatMessagesApp.Core.Domain.Common;

namespace ChatMessagesApp.Core.Domain.Events;

public class ParticipantAddedEvent : DomainEvent
{
    public Guid ConversationId { get; }
    public string AddedUserId { get; }
    public List<string> ExistingParticipantIds { get; }

    public ParticipantAddedEvent(Guid conversationId, string addedUserId, List<string> existingParticipantIds)
    {
        ConversationId = conversationId;
        AddedUserId = addedUserId;
        ExistingParticipantIds = existingParticipantIds;
        DateOccurred = DateTimeOffset.UtcNow;
    }
}
