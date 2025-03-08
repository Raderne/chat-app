using ChatMessagesApp.Core.Domain.Common;

namespace ChatMessagesApp.Core.Domain.Entities;

public class Demand : BaseEntity<Guid>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string CreatedByUserId { get; set; }

    public Demand(string title, string description, string createdByUserId)
    {
        Title = title;
        Description = description;
        CreatedByUserId = createdByUserId;

        AddDomainEvent(new DemandCreatedEvent(this));
    }

    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(DomainEvent domainItem) => _domainEvents.Add(domainItem);
    public void ClearDomainEvents() => _domainEvents.Clear();
}
