using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Application.Models.Notification;
using ChatMessagesApp.Core.Domain.Enums;
using ChatMessagesApp.Core.Domain.Events;
using MediatR;

namespace ChatMessagesApp.Core.Application.Handlers.Notifications;

public class DemandCreatedEventHandler(ISignalRService notificationService)
    : INotificationHandler<DomainEventNotification<DemandCreatedEvent>>
{
    private readonly ISignalRService _notificationService = notificationService;

    public async Task Handle(DomainEventNotification<DemandCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var demand = notification.DomainEvent.CreatedDemand;

        await _notificationService.NotifyUserAsync(
            demand.ToUserId,
            NotificationType.DemandCreated,
            $"Demand {demand.Title} has been created.");
    }
}

//public abstract class CreatedEventHandler<TEvent>(ISignalRService notificationService)
//    : INotificationHandler<DomainEventNotification<TEvent>> where TEvent : DomainEvent
//{
//    private readonly ISignalRService _notificationService = notificationService;

//    protected abstract string GetNotificationMessage(TEvent domainEvent);
//    protected abstract NotificationType GetNotificationType();

//    public async Task Handle(DomainEventNotification<TEvent> notification, CancellationToken cancellationToken)
//    {
//        var domainEvent = notification.DomainEvent;

//        await _notificationService.NotifyUserAsync(
//            GetUserId(domainEvent),
//            GetNotificationType(),
//            GetNotificationMessage(domainEvent));
//    }

//    protected abstract string GetUserId(TEvent domainEvent);
//}

//public class DemandAcceptedEventHandler(ISignalRService notificationService)
//    : CreatedEventHandler<DemandCreatedEvent>(notificationService)
//{
//    protected override string GetNotificationMessage(DemandCreatedEvent domainEvent)
//    {
//        var demand = domainEvent.CreatedDemand;
//        return $"Demand {demand.Title} has been accepted.";
//    }
//    protected override NotificationType GetNotificationType() => NotificationType.DemandCreated;
//    protected override string GetUserId(DemandCreatedEvent domainEvent) => domainEvent.CreatedDemand.CreatedBy;
//}