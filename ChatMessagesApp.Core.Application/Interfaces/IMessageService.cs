using ChatMessagesApp.Core.Domain.Entities;

namespace ChatMessagesApp.Core.Application.Interfaces;

public interface IMessageService
{
    Task<Message> AddAsync(Message message, CancellationToken cancellationToken);
    Task<Message> GetByIdAsync(Guid id);
    Task<List<Message>> GetByDemandIdAsync(Guid demandId);
    Task UpdateAsync(Message message, CancellationToken cancellationToken);
}

