namespace ChatMessagesApp.Core.Application.Features.Messages.Queries.GetConversation;

public record GetConversationDto(Guid DemandId, List<MessageDto> Messages);