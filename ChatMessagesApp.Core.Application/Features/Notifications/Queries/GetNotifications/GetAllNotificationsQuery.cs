using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Application.Responses;
using ChatMessagesApp.Core.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatMessagesApp.Core.Application.Features.Notifications.Queries.GetNotifications;

public class GetAllNotificationsQuery : IRequest<PaginatedResult<GetAllNotificationsDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetAllNotificationsQueryHandler(IContext context, ICurrentUserService currentUser) : IRequestHandler<GetAllNotificationsQuery, PaginatedResult<GetAllNotificationsDto>>
{
    private readonly IContext _context = context;
    private readonly ICurrentUserService _currentUser = currentUser;
    public async Task<PaginatedResult<GetAllNotificationsDto>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.IsLoggedIn() ? _currentUser.UserId : null;

        if (userId == null)
        {
            throw new ArgumentNullException(nameof(_currentUser.UserId));
        }

        var query = _context.Notifications
            .Where(c => c.UserId == userId)
            .Select(c => new GetAllNotificationsDto
            {
                Id = c.Id,
                Message = c.Message,
                ReceiverId = c.UserId,
                TransmitterId = c.CreatedBy,
                Type = c.Type,
                RequestId = c.RelatedDocumentId,
                IsRead = c.IsRead,
                TimeStamp = c.Created
            }).OrderBy(c => c.IsRead).ThenByDescending(c => c.TimeStamp);

        var totalUnseenNotification = await query.Where(n => n.IsRead == false).CountAsync(cancellationToken);
        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var totalItems = await query.CountAsync(cancellationToken);
        var paginatedResult = new PaginatedResult<GetAllNotificationsDto>(items, totalItems, request.PageNumber, request.PageSize)
        {
            TotalUnseenNotification = totalUnseenNotification
        };
        return paginatedResult;
    }
}

public class GetAllNotificationsDto
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public string ReceiverId { get; set; }
    public string TransmitterId { get; set; }
    public NotificationType Type { get; set; }
    public Guid? RequestId { get; set; }
    public bool IsRead { get; set; }
    public DateTime TimeStamp { get; set; }
}