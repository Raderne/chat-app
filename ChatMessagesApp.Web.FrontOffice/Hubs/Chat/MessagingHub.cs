using ChatMessagesApp.Core.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace ChatMessagesApp.Web.FrontOffice.Hubs.Chat;

public class MessagingHub : Hub<IMessagingHubClient>
{

}
