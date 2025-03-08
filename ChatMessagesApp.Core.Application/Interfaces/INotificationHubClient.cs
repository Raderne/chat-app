using ChatMessagesApp.Core.Application.Models.Notification;

namespace ChatMessagesApp.Core.Application.Interfaces;

public interface INotificationHubClient
{
    Task ReceiveNotification(NotificationDto notificationDto);
}

