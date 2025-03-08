using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Application.Models.Notification;
using ChatMessagesApp.Core.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChatMessagesApp.Infrastructure.Services;

public class DomainEventService(ILogger<DomainEventService> logger, IMediator mediator) : IDomainEventService
{
    private readonly ILogger<DomainEventService> _logger = logger;
    private readonly IMediator _mediator = mediator;

    public async Task Publish(DomainEvent domainEvent)
    {
        _logger.LogInformation("Publishing domain event. Event - {event}", domainEvent.GetType().Name);
        await _mediator.Publish(GetNotificationCorrespondingToDomainEvent(domainEvent));
    }

    private static INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent)
    {
        var notificationType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
        var notificationInstance = Activator.CreateInstance(notificationType, domainEvent) as INotification;

        if (notificationInstance == null)
        {
            throw new InvalidOperationException($"Could not create notification instance for domain event type {domainEvent.GetType().Name}");
        }

        return notificationInstance;
    }
}
