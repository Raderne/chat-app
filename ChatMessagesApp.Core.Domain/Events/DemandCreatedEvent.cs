using ChatMessagesApp.Core.Domain.Common;
using ChatMessagesApp.Core.Domain.Entities;

namespace ChatMessagesApp.Core.Domain.Events;

public class DemandCreatedEvent : DomainEvent
{
    public DemandCreatedEvent(Demand demand, Notification notification)
    {
        CreatedDemand = demand;
        Notification = notification;
        DateOccurred = DateTimeOffset.UtcNow;
    }
    public Demand CreatedDemand { get; }
    public Notification Notification { get; }
}
