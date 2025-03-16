using ChatMessagesApp.Core.Application.Features.Messages.Commands;
using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Application.Models.Notification;
using ChatMessagesApp.Core.Domain.Enums;
using ChatMessagesApp.Web.FrontOffice.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ChatMessagesApp.Infrastructure.Services;

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
        ArgumentNullException.ThrowIfNullOrEmpty(role, nameof(role));

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
        ArgumentNullException.ThrowIfNullOrEmpty(userId, nameof(userId));

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

    public async Task SendMessage(Guid demandId, string recipientId, string message)
    {
        var senderId = _currentUserService.UserId;

        var result = await _mediator.Send(new SendMessageCommand(demandId, message, senderId, recipientId));

        if (result.IsFailure)
        {
            throw new HubException(result.Error);
        }

        var recipientConnections = _userConnections.GetConnections(recipientId);
        if (recipientConnections != null)
        {
            foreach (var connectionId in recipientConnections)
            {
                await _hubContext.Clients.Client(connectionId).ReceiveMessage(result.Value!);
            }
        }

        // Also send back to sender for UI sync
        var senderConnections = _userConnections.GetConnections(senderId);
        if (senderConnections != null)
        {
            foreach (var connectionId in senderConnections)
            {
                await _hubContext.Clients.Client(connectionId).ReceiveMessage(result.Value!);
            }
        }
    }
}
