using ChatMessagesApp.Core.Domain.Common;

namespace ChatMessagesApp.Core.Domain.Entities;

public class Message : BaseEntity<Guid>
{
    public Guid ConversationId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string SenderId { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; }
    public Conversation Conversation { get; set; }
}
