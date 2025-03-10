namespace ChatMessagesApp.Core.Application.Interfaces;

public interface IMessagingHubClient
{
    Task SendMessage(Guid demandId, string content);
    Task JoinConversation(Guid demandId);
}

