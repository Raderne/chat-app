using ChatMessagesApp.Core.Domain.Entities;

namespace ChatMessagesApp.Core.Application.Interfaces;

public interface IConversationService
{
    Task<Conversation> GetByIdAsync(Guid id);
    Task<Conversation> GetByDemandIdAsync(Guid demandId);
    Task AddAsync(Conversation conversation);
    Task UpdateAsync(Conversation conversation);
}

