using ChatMessagesApp.Core.Application.Features;
using ChatMessagesApp.Core.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ChatMessagesApp.Web.FrontOffice.Hubs;

public class SignalRHub(
    IUserConnectionManager userConnectionManager,
    ICurrentUserService currentUser,
    IMediator mediator) : Hub<IHubClient>
{
    private readonly IUserConnectionManager _connectionManager = userConnectionManager;
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly IMediator _mediator = mediator;

    public async Task MarkAsRead(List<Guid> notificationIds)
    {
        var userId = _currentUser?.IsLoggedIn() == true ? _currentUser.UserId : null;
        if (userId == null)
        {
            throw new ArgumentNullException(nameof(_currentUser.UserId));
        }
        await _mediator.Send(new MarkNotificationsAsReadCommand(notificationIds, userId!));
    }

    public override async Task OnConnectedAsync()
    {
        var userId = _currentUser?.UserId;
        //var roles = _currentUser?.Roles;

        if (!string.IsNullOrEmpty(userId))
        {
            _connectionManager.AddConnection(userId, Context.ConnectionId);

            //if (roles != null)
            //{
            //    foreach (var role in roles)
            //    {
            //        await Groups.AddToGroupAsync(Context.ConnectionId, role);
            //    }
            //}
        }


        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = _currentUser?.UserId;
        if (!string.IsNullOrEmpty(userId))
            _connectionManager.RemoveConnection(userId!, Context.ConnectionId);

        await base.OnDisconnectedAsync(exception);
    }
}
