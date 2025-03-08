using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Application.Models.Notification;
using ChatMessagesApp.Core.Domain.Common;
using ChatMessagesApp.Core.Domain.Enums;
using MediatR;

namespace ChatMessagesApp.Core.Application.Handlers.Notifications;

public class DemandCreatedEventHandler(
    INotificationService notificationService,
    IContext context) : INotificationHandler<DomainEventNotification<DemandCreatedEvent>>
{
    private readonly INotificationService _notificationService = notificationService;
    private readonly IContext _context = context;

    public async Task Handle(DomainEventNotification<DemandCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var demand = notification.DomainEvent.CreatedDemand;

        await _notificationService.NotifyUserAsync(
            demand.CreatedBy,
            NotificationType.DemandCreated,
            $"Demand {demand.Title} has been created.");
    }
}
