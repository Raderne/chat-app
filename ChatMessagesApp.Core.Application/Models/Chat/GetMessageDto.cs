namespace ChatMessagesApp.Core.Application.Models;

public record GetMessageDto(
    Guid Id,
    Guid ConversationId,
    string SenderId,
    string Content,
    DateTime Created
);