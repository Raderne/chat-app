using ChatMessagesApp.Core.Domain.Events;

namespace ChatMessagesApp.Core.Domain.Entities;

public class Demand : EntityWithDomainEvents<Guid>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ToUserId { get; set; }

    public Demand(string title, string description, string toUserId)
    {
        Title = title;
        Description = description;
        ToUserId = toUserId;

        AddDomainEvent(new DemandCreatedEvent(this));
    }

    //private readonly List<DomainEvent> _domainEvents = new();
    //public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    //public void AddDomainEvent(DomainEvent domainItem) => _domainEvents.Add(domainItem);
    //public void ClearDomainEvents() => _domainEvents.Clear();
}
