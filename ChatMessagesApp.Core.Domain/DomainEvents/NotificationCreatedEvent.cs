using ChatMessagesApp.Core.Domain.Common;
using ChatMessagesApp.Core.Domain.Entities;

namespace ChatMessagesApp.Core.Domain.DomainEvents;

public class NotificationCreatedEvent : DomainEvent
{
    public Notification Notification { get; }

    public NotificationCreatedEvent(Notification notification)
    {
        Notification = notification;
    }
}
