namespace ChatMessagesApp.Core.Application.Models;

public record GetMessageDto(
    Guid Id,
    Guid ConversationId,
    string SenderId,
    string Content,
    DateTime Created,
    string CreatedBy
);

public record GetConversationDto(
    Guid Id,
    Guid DemandId,
    List<string> ParticipantIds,
    List<GetMessageDto> Messages
);