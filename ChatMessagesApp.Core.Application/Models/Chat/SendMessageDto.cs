namespace ChatMessagesApp.Core.Application.Models;

public record SendMessageDto(Guid DemandId, string SendToId, Guid ConversationId, string Content);
