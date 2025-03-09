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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Ignore<DomainEvent>();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        string userId = null;
        if (_currentUserService.IsLoggedIn())
        {
            userId = $"{_currentUserService.UserId}:{_currentUserService.UserName}:{_currentUserService.PreferedUserName}";
        }

        foreach (var entry in ChangeTracker.Entries<IAuditableEntity<string>>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = userId ?? "Anonymous";
                    entry.Entity.Created = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = userId ?? "Anonymous";
                    entry.Entity.LastModified = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = userId ?? "Anonymous";
                    entry.Entity.LastModified = DateTime.UtcNow;
                    break;
                default:
                    break;
            }
        }

        var domainEventEntities = ChangeTracker.Entries<Demand>()
            .Select(po => po.Entity)
            .Where(po => po.DomainEvents.Any())
            .ToArray();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var entity in domainEventEntities)
        {
            var latestDomainEvent = entity.DomainEvents.LastOrDefault();
            if (latestDomainEvent != null)
            {
                await _domainEventService.Publish(latestDomainEvent);
                entity.ClearDomainEvents();
            }
        }

        return result;
    }
}
