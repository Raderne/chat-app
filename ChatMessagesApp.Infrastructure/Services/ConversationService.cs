using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatMessagesApp.Infrastructure.Services;

public class ConversationService(IContext context) : IConversationService
{
    private readonly IContext _context = context;
    public Task<Conversation> AddAsync(Conversation conversation, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Conversation> GetByDemandIdAsync(Guid demandId)
    {
        return await _context.Conversations.Where(c => c.DemandId == demandId).FirstOrDefaultAsync()
            ?? throw new Exception("Conversation not found");
    }

    public async Task<Conversation> GetByIdAsync(Guid id)
    {
        return await _context.Conversations.Where(c => c.Id == id).FirstOrDefaultAsync() ?? throw new Exception("Conversation not found");
    }

    public Task UpdateAsync(Conversation conversation, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
