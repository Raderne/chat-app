using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Application.Models.Notification;
using ChatMessagesApp.Core.Domain.Events;

namespace ChatMessagesApp.Core.Application.Handlers.Notifications;

public class DemandCreatedEventHandler
    : AbstractNotificationEventHandler<DemandCreatedEvent>
{
    public DemandCreatedEventHandler(ISignalRService notificationService) : base(notificationService)
    {
    }

    protected override NotificationDto GetNotification(DemandCreatedEvent domainEvent)
    {
        return new NotificationDto
        {
            Type = domainEvent.Notification.Type,
            Message = domainEvent.Notification.Message,
            DocumentId = domainEvent.CreatedDemand.Id,
            TimeStamp = domainEvent.CreatedDemand.Created
        };
    }

    protected override string GetRecipientId(DemandCreatedEvent domainEvent) => domainEvent.CreatedDemand.ToUserId;

}
