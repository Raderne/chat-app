using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Application.Models.Notification;
using ChatMessagesApp.Core.Domain.Common;
using MediatR;

namespace ChatMessagesApp.Core.Application.Handlers;

public abstract class AbstractNotificationEventHandler<TEvent>
    : INotificationHandler<DomainEventNotification<TEvent>> where TEvent : DomainEvent
{
    private readonly ISignalRService _notificationService;

    protected AbstractNotificationEventHandler(ISignalRService notificationService)
    {
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
    }

    public async Task Handle(DomainEventNotification<TEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        await _notificationService.NotifyUserAsync(
            GetNotification(domainEvent),
            GetRecipientId(domainEvent)
        );
    }

    protected abstract string GetRecipientId(TEvent domainEvent);
    protected abstract NotificationDto GetNotification(TEvent domainEvent);
}
