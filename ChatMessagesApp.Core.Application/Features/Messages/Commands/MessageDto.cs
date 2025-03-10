namespace ChatMessagesApp.Core.Application.Features;

public record MessageDto(Guid Id, Guid DemandId, DateTime SentAt, bool IsRead)
{
    public string Content { get; set; } = string.Empty;
    public string SenderId { get; set; } = string.Empty;
}