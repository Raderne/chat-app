using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Domain.Enums;
using ChatMessagesApp.Core.Domain.Events;

namespace ChatMessagesApp.Core.Application.Handlers.Notifications;

public class DemandCreatedEventHandler
    : AbstractNotificationEventHandler<DemandCreatedEvent>
{
    public DemandCreatedEventHandler(ISignalRService notificationService) : base(notificationService)
    {
    }

    protected override string GetNotificationMessage(DemandCreatedEvent domainEvent)
    {
        var demand = domainEvent.CreatedDemand;
        return $"Demand {demand.Title} has been created.";
    }

    protected override NotificationType GetNotificationType() => NotificationType.DemandCreated;

    protected override string GetRecipientId(DemandCreatedEvent domainEvent) => domainEvent.CreatedDemand.ToUserId;

    protected override Guid? GetRequestId(DemandCreatedEvent domainEvent) => domainEvent.CreatedDemand.Id;
}
