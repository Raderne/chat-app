using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Application.Models;
using ChatMessagesApp.Core.Application.Responses;
using MediatR;

namespace ChatMessagesApp.Core.Application.Features.Messages.Commands;

public record AddParticipantCommand(
    Guid ConversationId,
    string ParticipantId
) : IRequest<Result<AddParticipantDto>>;

public class AddParticipantCommandHandler(IConversationService conversationService)
    : IRequestHandler<AddParticipantCommand, Result<AddParticipantDto>>
{
    private readonly IConversationService _conversationService = conversationService;

    public async Task<Result<AddParticipantDto>> Handle(AddParticipantCommand request, CancellationToken cancellationToken)
    {
        var conversation = await _conversationService.GetByIdAsync(request.ConversationId);
        if (conversation == null)
            return Result<AddParticipantDto>.Failure("Conversation not found.");

        conversation.ParticipantIds.Add(request.ParticipantId);
        await _conversationService.UpdateAsync(conversation, cancellationToken);

        return Result<AddParticipantDto>.Success(new AddParticipantDto(request.ConversationId, request.ParticipantId));
    }
}