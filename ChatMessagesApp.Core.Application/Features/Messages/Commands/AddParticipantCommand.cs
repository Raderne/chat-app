using ChatMessagesApp.Core.Application.Models;
using ChatMessagesApp.Core.Application.Responses;
using MediatR;

namespace ChatMessagesApp.Core.Application.Features.Messages.Commands;

public record AddParticipantCommand(
    Guid ConversationId,
    List<string> ParticipantIds
) : IRequest<Result<AddParticipantDto>>;

public class AddParticipantCommandHandler : IRequestHandler<AddParticipantCommand, Result<AddParticipantDto>>
{

    public async Task<Result<AddParticipantDto>> Handle(AddParticipantCommand request, CancellationToken cancellationToken)
    {
        //var conversation = await _chatRepository.GetConversationByIdAsync(request.ConversationId);
        //if (conversation == null)
        //    return Result<AddParticipantDto>.Failure("Conversation not found.");
        //var participants = await _chatRepository.GetParticipantsByIdsAsync(request.ParticipantIds);
        //if (participants.Count != request.ParticipantIds.Count)
        //    return Result<AddParticipantDto>.Failure("Some participants not found.");
        //conversation.AddParticipants(participants);
        //await _chatRepository.SaveChangesAsync();
        //return Result<AddParticipantDto>.Success(new AddParticipantDto());
        return Result<AddParticipantDto>.Success(new AddParticipantDto(request.ConversationId, request.ParticipantIds));
    }
}