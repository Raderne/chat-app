using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Application.Models.Notification;
using ChatMessagesApp.Core.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ChatMessagesApp.Web.FrontOffice.Hubs;

public class SignalRService(
    IHubContext<SignalRHub, IHubClient> hubContext,
    IUserConnectionManager userConnections,
    IMediator mediator,
    ICurrentUserService currentUserService) : ISignalRService
{
    private readonly IHubContext<SignalRHub, IHubClient> _hubContext = hubContext;
    private readonly IUserConnectionManager _userConnections = userConnections;
    private readonly IMediator _mediator = mediator;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task NotifyRoleAsync(string role, NotificationType type, string message, Guid? documentId = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(role, nameof(role));

        var notification = new NotificationDto()
        {
            Type = type,
            Message = message,
            DocumentId = documentId
        };

        await _hubContext.Clients.Group(role).ReceiveNotification(notification);
    }

    public async Task NotifyUserAsync(string userId, NotificationType type, string message, Guid? documentId = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(userId, nameof(userId));

        var notification = new NotificationDto()
        {
            Type = type,
            Message = message,
            DocumentId = documentId,
            TimeStamp = DateTime.UtcNow
        };

        var connectionIds = _userConnections.GetConnections(userId);
        foreach (var connection in connectionIds)
        {
            await _hubContext.Clients.Client(connection).ReceiveNotification(notification);
        }

        //var not = new Notification()
        //{
        //    UserId = userId,
        //    Type = type,
        //    Message = $"You have a new demand from {_currentUserService.UserName}",
        //    RelatedDocumentId = demand.Id
        //};
        //_context.Notifications.Add(not);
    }

    public async Task SendMessage(string recipientId, string message)
    {
        var senderId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(recipientId))
        {
            throw new HubException("Recipient not found");
        }

        var recipientConnections = _userConnections.GetConnections(recipientId);
        if (recipientConnections != null)
        {
            foreach (var connectionId in recipientConnections)
            {
                await _hubContext.Clients.Client(connectionId).ReceiveMessage(message);
            }
        }

        // Also send back to sender for UI sync
        var senderConnections = _userConnections.GetConnections(senderId);
        if (senderConnections != null)
        {
            foreach (var connectionId in senderConnections)
            {
                await _hubContext.Clients.Client(connectionId).ReceiveMessage(message);
            }
        }
    }
}
