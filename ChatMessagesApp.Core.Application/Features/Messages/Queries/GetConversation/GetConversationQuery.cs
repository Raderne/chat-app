using ChatMessagesApp.Core.Application.Interfaces;
using MediatR;

namespace ChatMessagesApp.Core.Application.Features.Messages.Queries.GetConversation;

public record GetConversationQuery(Guid DemandId) : IRequest<GetConversationDto>;

public class GetConversationQueryHandler(IConversationService conversationService) : IRequestHandler<GetConversationQuery, GetConversationDto>
{
    private readonly IConversationService _conversationService = conversationService;

    public async Task<GetConversationDto> Handle(GetConversationQuery request, CancellationToken cancellationToken)
    {
        var conversation = await _conversationService.GetByDemandIdAsync(request.DemandId);
        return new GetConversationDto(
            conversation.DemandId,
            conversation.Messages
            .Select(m => new MessageDto(m.Id, m.DemandId, m.SentAt, m.IsRead)
            {
                Content = m.Content,
                SenderId = m.SenderId
            }).ToList());
    }
}
