namespace ChatMessagesApp.Core.Application.Models;

public record AddParticipantDto(Guid ConversationId, string ParticipantId);
