using ChatMessagesApp.Core.Domain.Common;

namespace ChatMessagesApp.Core.Domain.Entities;

public class Message : BaseEntity<Guid>
{
    public Guid DemandId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string SenderId { get; set; }
    public string RecipientId { get; set; }
    public bool IsRead { get; set; }
}
