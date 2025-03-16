namespace ChatMessagesApp.Core.Application.Models;

public record SendMessageDto(Guid DemandId, string SendToId, string Content);
