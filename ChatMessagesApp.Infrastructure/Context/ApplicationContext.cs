﻿using ChatMessagesApp.Core.Application.Interfaces;
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
    public DbSet<Conversation> Conversations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Message>()
            .HasIndex(m => m.MessageStatus)
            .HasDatabaseName("IX_Message_MessageStatus");

        modelBuilder.Entity<Message>()
            .HasIndex(m => m.DemandId)
            .HasDatabaseName("IX_Message_DemandId");

        modelBuilder.Entity<Message>()
            .HasIndex(m => m.SenderId)
            .HasDatabaseName("IX_Message_SenderId");

        modelBuilder.Entity<Message>()
            .HasIndex(m => m.ConversationId)
            .HasDatabaseName("IX_Message_ConversationId");

        modelBuilder.Entity<Message>()
            .HasIndex(m => new { m.SenderId, m.ConversationId })
            .HasDatabaseName("IX_Message_SenderId_ConversationId");

        modelBuilder.Entity<Message>()
            .HasIndex(m => m.Created)
            .HasDatabaseName("IX_Message_Created");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return SaveChangesAsync(null, cancellationToken);
    }

    public virtual async Task<int> SaveChangesAsync(Action<Exception>? onException = null, CancellationToken cancellationToken = default, string tenantId = null)
    {
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

            //await PublishDomainEventsAsync();
            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEventEntities = ChangeTracker.Entries<EntityWithDomainEvents<Guid>>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToArray();

        foreach (var entity in domainEventEntities)
        {
            //var latestDomainEvent = entity.DomainEvents.LastOrDefault();
            var events = entity.DomainEvents.ToArray();
            entity.ClearDomainEvents();
            foreach (var domainEvent in events)
            {
                await _domainEventService.Publish(domainEvent);
            }
        }
    }
}
