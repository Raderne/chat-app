using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatMessagesApp.Infrastructure.Services;

public class MessageService(IContext context) : IMessageService
{
    private readonly IContext _context = context;

    public async Task<Message> AddAsync(Message message, CancellationToken cancellationToken)
    {
        await _context.Messages.AddAsync(message, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return message;
    }

    public async Task<List<Message>> GetByDemandIdAsync(Guid demandId)
    {
        return await _context.Messages.Where(m => m.DemandId == demandId).ToListAsync();
    }

    public async Task<Message> GetByIdAsync(Guid id)
    {
        return await _context.Messages.Where(m => m.Id == id).FirstOrDefaultAsync() ?? throw new Exception("Message not found");
    }

    public Task UpdateAsync(Message message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
