using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Application.Models.Notification;
using ChatMessagesApp.Core.Domain.Enums;
using ChatMessagesApp.Web.FrontOffice.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ChatMessagesApp.Infrastructure.Services;

public class SignalRNotificationService(
    IHubContext<NotificationHub, INotificationHubClient> hubContext,
    IUserConnectionManager userConnections,
    IContext context) : INotificationService
{
    private readonly IHubContext<NotificationHub, INotificationHubClient> _hubContext = hubContext;
    private readonly IUserConnectionManager _userConnections = userConnections;
    private readonly IContext _context = context;

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
}
