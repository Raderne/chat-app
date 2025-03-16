namespace ChatMessagesApp.Core.Application.Models;

public record GetMessageDto(
    Guid Id,
    string SenderId,
    string RecipientId,
    string Content,
    DateTime Created
);