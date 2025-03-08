using ChatMessagesApp.Core.Domain.Common;
using ChatMessagesApp.Core.Domain.Enums;

namespace ChatMessagesApp.Core.Domain.Entities;

public class Notification : BaseEntity<Guid>
{
    public string UserId { get; set; } // Target user
    public NotificationType Type { get; set; }
    public string Message { get; set; }
    public Guid? RelatedDocumentId { get; set; } // Optional link to documents
    public bool IsRead { get; set; }
}
