using ChatMessagesApp.Core.Domain.Entities;

namespace ChatMessagesApp.Core.Application.Interfaces;

public interface IConversationService
{
    Task<Conversation> GetByDemandIdAsync(Guid demandId);
    Task UpdateAsync(Conversation conversation);
}

