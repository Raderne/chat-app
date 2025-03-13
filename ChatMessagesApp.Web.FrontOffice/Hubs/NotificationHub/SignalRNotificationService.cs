using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Application.Models.Notification;
using ChatMessagesApp.Core.Domain.Enums;
using ChatMessagesApp.Web.FrontOffice.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ChatMessagesApp.Infrastructure.Services;

public class SignalRNotificationService(
    IHubContext<NotificationHub, INotificationHubClient> hubContext,
    IUserConnectionManager userConnections) : INotificationService
{
    private readonly IHubContext<NotificationHub, INotificationHubClient> _hubContext = hubContext;
    private readonly IUserConnectionManager _userConnections = userConnections;

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
    }
}
