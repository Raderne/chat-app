using ChatMessagesApp.Core.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatMessagesApp.Web.FrontOffice.Hubs.NotificationHub;

public class NotificationHub(
    IUserConnectionManager userConnectionManager,
    ICurrentUserService currentUser) : Hub<INotificationHubClient>
{
    private readonly IUserConnectionManager _connectionManager = userConnectionManager;
    private readonly ICurrentUserService _currentUser = currentUser;

    public override async Task OnConnectedAsync()
    {
        var userId = _currentUser?.UserId;
        var roles = _currentUser?.Roles;

        if (!string.IsNullOrEmpty(userId))
        {
            _connectionManager.AddConnection(userId, Context.ConnectionId);

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, role);
                }
            }
        }


        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _connectionManager.RemoveConnection(userId!, Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}
