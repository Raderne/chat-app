using ChatMessagesApp.Core.Domain.Entities;

namespace ChatMessagesApp.Core.Application.Interfaces;

public interface IConversationService
{
    Task<Conversation> AddAsync(Conversation conversation, CancellationToken cancellationToken);
    Task<Conversation> GetByIdAsync(Guid id);
    Task<Conversation> GetByDemandIdAsync(Guid demandId);
    Task UpdateAsync(Conversation conversation, CancellationToken cancellationToken);
}
