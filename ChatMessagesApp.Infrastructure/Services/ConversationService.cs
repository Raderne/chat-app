using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatMessagesApp.Infrastructure.Services;

public class ConversationService(IContext context) : IConversationService
{
    private readonly IContext _context = context;
    public async Task<Conversation> AddAsync(Conversation conversation, CancellationToken cancellationToken)
    {
        try
        {
            _context.Conversations.Add(conversation);
            return await _context.SaveChangesAsync(cancellationToken) > 0
                ? conversation
                : throw new Exception("Failed to add conversation");
        }
        catch (Exception)
        {
            throw new Exception("Failed to add conversation");
        }
    }

    public async Task<Conversation> GetByDemandIdAsync(Guid demandId)
    {
        return await _context.Conversations
            .Include(c => c.Messages)
            .Where(c => c.DemandId == demandId)
            .FirstOrDefaultAsync() ?? throw new Exception("Conversation not found");
    }

    public async Task<Conversation> GetByIdAsync(Guid id)
    {
        return await _context.Conversations
            .Include(c => c.Messages)
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync() ?? throw new Exception("Conversation not found");
    }

    public async Task UpdateAsync(Conversation conversation, CancellationToken cancellationToken)
    {
        try
        {
            _context.Conversations.Update(conversation);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            throw new Exception("Failed to update conversation");
        }
    }
}
