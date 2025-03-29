namespace ChatMessagesApp.Core.Application.Models;

public record SendMessageDto(Guid DemandId, Guid ConversationId, string Content);
