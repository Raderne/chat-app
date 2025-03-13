namespace ChatMessagesApp.Core.Application.Models.Chat;

public record SendMessageDto(
    Guid Id,
    string SenderId,
    string RecipientId,
    string Content,
    DateTime Created
);