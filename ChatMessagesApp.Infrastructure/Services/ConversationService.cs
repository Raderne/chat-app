using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatMessagesApp.Infrastructure.Services;

public class ConversationService(IContext context) : IConversationService
{
    private readonly IContext _context = context;

    public Task AddAsync(Conversation conversation)
    {
        throw new NotImplementedException();
    }

    public async Task<Conversation> GetByDemandIdAsync(Guid demandId)
    {
        return await _context.Conversations
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.DemandId == demandId) ?? throw new Exception("Conversation not found");
    }

    public Task<Conversation> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Conversation conversation)
    {
        _context.Conversations.Update(conversation);
        return _context.SaveChangesAsync();
    }
}
