using ChatMessagesApp.Core.Application.Features.Messages.Commands;
using ChatMessagesApp.Core.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ChatMessagesApp.Web.FrontOffice.Hubs;

public class MessagingHub(
    IMediator mediator,
    ICurrentUserService currentUserService,
    IUserConnectionManager userConnectionManager) : Hub<IMessagingHubClient>
{
    private readonly IMediator _mediator = mediator;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IUserConnectionManager _userConnectionManager = userConnectionManager;

    public async Task SendMessage(Guid demandId, string recipientId, string message)
    {
        var senderId = _currentUserService.UserId;

        var result = await _mediator.Send(new SendMessageCommand(demandId, message, senderId, recipientId));

        if (result.IsFailure)
        {
            throw new HubException(result.Error);
        }

        var recipientConnections = _userConnectionManager.GetConnections(recipientId);
        if (recipientConnections != null)
        {
            foreach (var connectionId in recipientConnections)
            {
                await Clients.Client(connectionId).ReceiveMessage(result.Value!);
            }
        }

        // Also send back to sender for UI sync
        var senderConnections = _userConnectionManager.GetConnections(senderId);
        if (senderConnections != null)
        {
            foreach (var connectionId in senderConnections)
            {
                await Clients.Client(connectionId).ReceiveMessage(result.Value!);
            }
        }
    }
}
