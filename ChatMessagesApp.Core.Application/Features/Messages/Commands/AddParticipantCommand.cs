using ChatMessagesApp.Core.Application.Responses;
using MediatR;

namespace ChatMessagesApp.Core.Application.Features.Messages.Commands;

public record AddParticipantCommand(
    Guid ConversationId,
    List<string> ParticipantIds
) : IRequest<Result<AddParticipantDto>>;
