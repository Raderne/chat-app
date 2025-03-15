using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Application.Models.Chat;
using ChatMessagesApp.Core.Application.Responses;
using ChatMessagesApp.Core.Domain.Entities;
using ChatMessagesApp.Core.Domain.Events;
using MediatR;

namespace ChatMessagesApp.Core.Application.Features.Messages.Commands;

public record SendMessageCommand(Guid DemandId, string Content, string SenderId, string RecipientId) : IRequest<Result<SendMessageDto>>;

public class SendMessageCommandHandler(
    IMessageService messageService,
    IDomainEventService domainEventService) : IRequestHandler<SendMessageCommand, Result<SendMessageDto>>
{
    private readonly IMessageService _messageService = messageService;
    private readonly IDomainEventService _domainEventService = domainEventService;

    public async Task<Result<SendMessageDto>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var message = new Message()
        {
            DemandId = request.DemandId,
            Content = request.Content,
            SenderId = request.SenderId,
            RecipientId = request.RecipientId,
            IsRead = false
        };

        try
        {
            Task addedMessage = _messageService.AddAsync(message, cancellationToken);

            Task sendMessageInRealTime = _domainEventService.Publish(new MessageSentEvent(message, message.RecipientId));

            await Task.WhenAll(addedMessage, sendMessageInRealTime);

            return Result<SendMessageDto>.Success(new SendMessageDto(
                message.Id,
                message.Content,
                message.SenderId,
                message.RecipientId,
                message.Created));
        }
        catch (Exception)
        {
            return Result<SendMessageDto>.Failure("Failed to send message");
        }
    }
}