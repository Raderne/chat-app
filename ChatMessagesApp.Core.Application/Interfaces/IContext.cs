using ChatMessagesApp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatMessagesApp.Core.Application.Interfaces;

public interface IContext
{
    public DbSet<Demand> Demands { get; }
    public DbSet<Notification> Notifications { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
}

