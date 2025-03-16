using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Application.Models.Notification;
using ChatMessagesApp.Core.Domain.Enums;
using ChatMessagesApp.Core.Domain.Events;
using MediatR;

namespace ChatMessagesApp.Core.Application.Handlers;

public class MessageSentEventHandler(ISignalRService notificationService) : INotificationHandler<DomainEventNotification<MessageSentEvent>>
{
    private readonly ISignalRService _notificationService = notificationService;

    public async Task Handle(DomainEventNotification<MessageSentEvent> notification, CancellationToken cancellationToken)
    {
        var message = notification.DomainEvent.MessageCreated;

        // Send notification to recipient
        await _notificationService.NotifyUserAsync(
            notification.DomainEvent.RecipientUserId,
            NotificationType.NewMessage,
            message.RecipientId + " has sent a message",
            message.DemandId);
    }
}
