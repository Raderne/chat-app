namespace ChatMessagesApp.Core.Application.Models;

public record MessageHubDto(Guid Id, string Content, string SenderId, DateTime SentAt);
