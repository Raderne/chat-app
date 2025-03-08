using ChatMessagesApp.Core.Domain.Entities;

namespace ChatMessagesApp.Core.Domain.Common;

public class DemandCreatedEvent : DomainEvent
{
    public DemandCreatedEvent(Demand demand)
    {
        CreatedDemand = demand;
        DateOccurred = DateTimeOffset.UtcNow;
    }
    public Demand CreatedDemand { get; }
}
