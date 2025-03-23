using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Application.Models.Notification;
using ChatMessagesApp.Core.Domain.Common;
using ChatMessagesApp.Core.Domain.Enums;
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
            GetRecipientId(domainEvent),
            GetNotificationType(),
            GetNotificationMessage(domainEvent),
            GetRequestId(domainEvent)
        );
    }

    protected abstract string GetRecipientId(TEvent domainEvent);
    protected abstract NotificationType GetNotificationType();
    protected abstract string GetNotificationMessage(TEvent domainEvent);
    protected abstract Guid? GetRequestId(TEvent domainEvent);
}
