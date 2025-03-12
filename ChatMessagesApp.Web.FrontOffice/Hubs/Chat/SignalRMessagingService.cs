using ChatMessagesApp.Core.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace ChatMessagesApp.Web.FrontOffice.Hubs.Chat;

public class SignalRMessagingService(
    IHubContext<MessagingHub, IMessagingHubClient> hubContext) : IMessagingService
{
    private readonly IHubContext<MessagingHub, IMessagingHubClient> _hubContext = hubContext;
    public Task JoinConversation(Guid demandId)
    {
        throw new NotImplementedException();
    }

    public Task LeaveConversation(Guid demandId)
    {
        throw new NotImplementedException();
    }

    public Task SendMessage(Guid demandId, string content)
    {
        throw new NotImplementedException();
    }
}
