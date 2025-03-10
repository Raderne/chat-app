using ChatMessagesApp.Core.Domain.Common;

namespace ChatMessagesApp.Core.Domain.Entities;

public class Conversation : BaseEntity<Guid>
{
    public Guid DemandId { get; set; }
    public List<string> ParticipantIds { get; set; }
    public List<Message> Messages { get; set; } = new();
}
