using ChatMessagesApp.Core.Domain.Common;

namespace ChatMessagesApp.Core.Domain.Entities;

public class Conversation : BaseEntity<Guid>
{
    public string Title { get; set; } = null!;
    public Guid DemandId { get; set; }
    public Demand Demand { get; set; } = null!;
    public List<string> ParticipantIds { get; set; } = new List<string>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();

}
