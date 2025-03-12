namespace ChatMessagesApp.Core.Application.Interfaces;

public interface IMessagingService
{
    Task SendMessage(Guid demandId, string content);
    Task JoinConversation(Guid demandId);
    Task LeaveConversation(Guid demandId);
}

