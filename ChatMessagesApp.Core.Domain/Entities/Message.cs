using ChatMessagesApp.Core.Domain.Common;
using ChatMessagesApp.Core.Domain.Enums;

namespace ChatMessagesApp.Core.Domain.Entities;

public class Message : BaseEntity<Guid>
{
    public Guid DemandId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string SenderId { get; set; }
    public MessageStatus MessageStatus { get; set; } = MessageStatus.Sent;
    public Guid? ConversationId { get; set; }
}
