using ChatMessagesApp.Core.Application.Models.Chat;

namespace ChatMessagesApp.Core.Application.Interfaces;

public interface IMessagingHubClient
{
    Task ReceiveMessage(SendMessageDto messageDto);
}