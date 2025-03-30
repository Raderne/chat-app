using AutoMapper;
using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Domain.Entities;
using ChatMessagesApp.Core.Domain.Enums;
using MediatR;

namespace ChatMessagesApp.Core.Application.Features.Demands.Commands.CreateDemand;

public class CreateDemandCommand : IRequest<CreateDemandDto>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string NotifyUserId { get; set; } = string.Empty;
}

public class CreateDemandCommandHandler(
    ICurrentUserService currentUserService,
    IContext context,
    IMapper mapper) : IRequestHandler<CreateDemandCommand, CreateDemandDto>
{
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IContext _context = context;

    public async Task<CreateDemandDto> Handle(CreateDemandCommand request, CancellationToken cancellationToken)
    {
        var demand = new Demand(request.Title, request.Description, request.NotifyUserId);
        _context.Demands.Add(demand);

        var conversation = new Conversation(
            request.Title,
            demand.Id,
            new List<string> { _currentUserService.UserId, request.NotifyUserId }
            );
        _context.Conversations.Add(conversation);

        var notification = new Notification()
        {
            UserId = request.NotifyUserId,
            Type = NotificationType.DemandCreated,
            Message = $"You have a new demand from {_currentUserService.UserName}",
            RelatedDocumentId = demand.Id
        };
        _context.Notifications.Add(notification);

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CreateDemandDto>(demand);
    }
}