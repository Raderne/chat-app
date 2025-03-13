using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Domain.Common;
using ChatMessagesApp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatMessagesApp.Infrastructure.Context;

public class ApplicationContext(
    DbContextOptions<ApplicationContext> options,
    IDomainEventService domainEventService,
    ICurrentUserService currentUserService) : DbContext(options), IContext
{
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IDomainEventService _domainEventService = domainEventService;

    public DbSet<Demand> Demands { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Ignore<DomainEvent>();

        modelBuilder.Entity<Message>()
            .HasIndex(m => m.IsRead)
            .HasDatabaseName("IX_Message_IsRead");

        modelBuilder.Entity<Message>()
            .HasIndex(m => m.DemandId)
            .HasDatabaseName("IX_Message_DemandId");

        modelBuilder.Entity<Message>()
            .HasIndex(m => m.SenderId)
            .HasDatabaseName("IX_Message_SenderId");

        modelBuilder.Entity<Message>()
            .HasIndex(m => m.RecipientId)
            .HasDatabaseName("IX_Message_RecipientId");

        modelBuilder.Entity<Message>()
            .HasIndex(m => new { m.SenderId, m.RecipientId })
            .HasDatabaseName("IX_Message_SenderId_RecipientId");

        modelBuilder.Entity<Message>()
            .HasIndex(m => m.Created)
            .HasDatabaseName("IX_Message_Created");
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        string userId = _currentUserService.IsLoggedIn()
            ? $"{_currentUserService.UserId}:{_currentUserService.UserName}:{_currentUserService.PreferedUserName}"
            : "Anonymous";

        var currentTime = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<IAuditableEntity<string>>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.Created = currentTime;
                    entry.Entity.LastModifiedBy = userId;
                    entry.Entity.LastModified = currentTime;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = userId;
                    entry.Entity.LastModified = currentTime;
                    break;
            }
        }

        var domainEventEntities = ChangeTracker.Entries<Demand>()
            .Select(d => d.Entity)
            .Where(d => d.DomainEvents.Any())
            .ToArray();

        foreach (var entity in domainEventEntities)
        {
            var latestDomainEvent = entity.DomainEvents.LastOrDefault();
            if (latestDomainEvent != null)
            {
                await _domainEventService.Publish(latestDomainEvent);
                entity.ClearDomainEvents();
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}
