using ChatMessagesApp.Core.Domain.Enums;

namespace ChatMessagesApp.Core.Application.Models.Notification;

public class NotificationDto
{
    public NotificationType Type { get; set; }
    public string Message { get; set; }
    public Guid? DocumentId { get; set; }
    public DateTime TimeStamp { get; set; }
}
