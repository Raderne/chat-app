using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Application.Models.Notification;
using ChatMessagesApp.Core.Domain.Enums;
using ChatMessagesApp.Core.Domain.Events;
using MediatR;

namespace ChatMessagesApp.Core.Application.Handlers.Notifications;

public class DemandCreatedEventHandler(INotificationService notificationService)
    : INotificationHandler<DomainEventNotification<DemandCreatedEvent>>
{
    private readonly INotificationService _notificationService = notificationService;

    public async Task Handle(DomainEventNotification<DemandCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var demand = notification.DomainEvent.CreatedDemand;

        await _notificationService.NotifyUserAsync(
            demand.CreatedByUserId,
            NotificationType.DemandCreated,
            $"Demand {demand.Title} has been created.");
    }
}
