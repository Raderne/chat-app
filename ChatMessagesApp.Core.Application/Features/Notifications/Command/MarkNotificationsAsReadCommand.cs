using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatMessagesApp.Core.Application.Features;

public record MarkNotificationsAsReadCommand(List<Guid> NotificationIds, string userId) : IRequest<Result>;

public class MarkNotificationsAsReadCommandHandler(IContext context)
    : IRequestHandler<MarkNotificationsAsReadCommand, Result>
{
    private readonly IContext _context = context;

    public async Task<Result> Handle(MarkNotificationsAsReadCommand command, CancellationToken cancellationToken)
    {
        var notifications = await _context.Notifications
            .Where(n => command.NotificationIds.Contains(n.Id) && n.UserId == command.userId)
            .ToListAsync(cancellationToken);

        if (notifications.Count == 0)
        {
            return Result.Failure(["No notifications found to mark as read."]);
        }

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
        }
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
