using ChatMessagesApp.Core.Domain.Enums;

namespace ChatMessagesApp.Core.Application.Interfaces;

public interface ISignalRService
{
    Task NotifyUserAsync(string userId, NotificationType type, string message, Guid? documentId = null);
    Task NotifyRoleAsync(string role, NotificationType type, string message, Guid? documentId = null);
    Task SendMessage(string recipiantId, string message);
}
