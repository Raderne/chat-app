using ChatMessagesApp.Core.Domain.Common;

namespace ChatMessagesApp.Core.Domain.Entities;

public class Conversation : BaseEntity<Guid>
{
    public Guid DemandId { get; set; }
    public string InitiatorUserId { get; set; }
    public string ReceiverUserId { get; set; }
    public Demand Demand { get; set; }
    public IEnumerable<Message> Messages { get; set; } = new List<Message>();
}
