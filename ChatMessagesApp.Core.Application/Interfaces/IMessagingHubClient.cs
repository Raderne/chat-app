using ChatMessagesApp.Core.Application.Models;

namespace ChatMessagesApp.Core.Application.Interfaces;

public interface IMessagingHubClient
{
    Task ReceiveMessage(MessageHubDto message);
}
