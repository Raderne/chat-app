using ChatMessagesApp.Core.Domain.Common;

namespace ChatMessagesApp.Core.Application.Interfaces;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}

