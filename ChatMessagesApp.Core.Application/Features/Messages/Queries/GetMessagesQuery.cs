using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Application.Models;
using ChatMessagesApp.Core.Application.Responses;
using MediatR;

namespace ChatMessagesApp.Core.Application.Features.Messages.Queries;

public record GetMessagesQuery(Guid DemandId, string UserId) : IRequest<Result<GetConversationDto>>;

public class GetMessagesQueryHandler(IConversationService conversationService) : IRequestHandler<GetMessagesQuery, Result<GetConversationDto>>
{
    private readonly IConversationService _conversationService = conversationService;
    public async Task<Result<GetConversationDto>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var conversation = await _conversationService.GetByDemandIdAsync(request.DemandId);
        if (conversation == null)
            return Result<GetConversationDto>.Failure("Messages not found.");

        var messages = conversation.Messages.Select(m => new GetMessageDto(
            m.Id,
            (Guid)m.ConversationId!,
            m.SenderId,
            m.Content,
            m.Created,
            m.CreatedBy
        )).ToList();

        var result = new GetConversationDto(
            conversation.Id,
            conversation.DemandId,
            conversation.ParticipantIds,
            messages
        );

        return Result<GetConversationDto>.Success(result);

    }
}
