using ChatMessagesApp.Core.Application.Models.Chat;
using ChatMessagesApp.Core.Application.Models.Notification;

namespace ChatMessagesApp.Core.Application.Interfaces;

public interface IHubClient
{
    Task ReceiveNotification(NotificationDto notificationDto);
    Task ReceiveMessage(SendMessageDto messageDto);
}
