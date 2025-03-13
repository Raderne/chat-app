using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Application.Models.Chat;
using MediatR;

namespace ChatMessagesApp.Core.Application.Features.Messages.Queries;

public record GetMessagesQuery(Guid DemandId, string UserId) : IRequest<List<SendMessageDto>>;

public class GetMessagesQueryHandler(IMessageService messageService) : IRequestHandler<GetMessagesQuery, List<SendMessageDto>>
{
    private readonly IMessageService _messageService = messageService;

    public async Task<List<SendMessageDto>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var messages = await _messageService.GetByDemandIdAsync(request.DemandId);
        return messages.Select(m => new SendMessageDto(m.Id, m.SenderId, m.RecipientId, m.Content, m.Created)).ToList();
    }
}