using ChatMessagesApp.Core.Domain.Common;

namespace ChatMessagesApp.Core.Domain.Entities;

public class NotificationCreatedEvent : DomainEvent
{
    public Notification Notification { get; }

    public NotificationCreatedEvent(Notification notification)
    {
        Notification = notification;
    }
}
