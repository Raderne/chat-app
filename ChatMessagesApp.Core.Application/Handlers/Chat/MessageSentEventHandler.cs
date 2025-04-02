using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Application.Models.Notification;
using ChatMessagesApp.Core.Domain.Enums;
using ChatMessagesApp.Core.Domain.Events;
using MediatR;

namespace ChatMessagesApp.Core.Application.Handlers;

public class MessageSentEventHandler(ISignalRService signalRService) : INotificationHandler<DomainEventNotification<MessageSentEvent>>
{
    private readonly ISignalRService _notificationService = signalRService;

    public async Task Handle(DomainEventNotification<MessageSentEvent> notification, CancellationToken cancellationToken)
    {
        var message = notification.DomainEvent.MessageCreated;
        var participantIds = notification.DomainEvent.ParticipantIds;

        // Send notification to recipient
        //await _notificationService.NotifyUserAsync(
        //    message.RecipientId,
        //    NotificationType.NewMessage,
        //    message.CreatedBy.Split(":")[1] + " sent you a message.",
        //    message.DemandId);

        //await _notificationService.SendMessage(message.RecipientId, message.Content);

        foreach (var participantId in participantIds)
        {
            if (participantId != message.SenderId)
            {
                var notificationDto = new NotificationDto()
                {
                    Type = NotificationType.NewMessage,
                    Message = message.CreatedBy.Split(":")[1] + " sent a message.",
                    DocumentId = message.DemandId
                };
                // Send notification to other participants
                await _notificationService.NotifyUserAsync(notificationDto, participantId);

                await _notificationService.SendMessage(participantId, message.Content);
            }
        }
    }
}
