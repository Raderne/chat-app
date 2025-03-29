namespace ChatMessagesApp.Core.Application.Models;

public record AddParticipantDto(Guid ConversationId, List<string> ParticipantIds);
