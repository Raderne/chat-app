using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Application.Models;
using ChatMessagesApp.Core.Application.Responses;
using ChatMessagesApp.Core.Domain.Entities;
using ChatMessagesApp.Core.Domain.Events;
using MediatR;

namespace ChatMessagesApp.Core.Application.Features.Messages.Commands;

public record SendMessageCommand(Guid DemandId, string Content, string RecipientId) : IRequest<Result<GetMessageDto>>;

public class SendMessageCommandHandler(
    IMessageService messageService,
    IDomainEventService domainEventService,
    ICurrentUserService currentUserService) : IRequestHandler<SendMessageCommand, Result<GetMessageDto>>
{
    private readonly IMessageService _messageService = messageService;
    private readonly IDomainEventService _domainEventService = domainEventService;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<Result<GetMessageDto>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var senderId = _currentUserService.UserId;

        var message = new Message()
        {
            DemandId = request.DemandId,
            Content = request.Content,
            SenderId = senderId,
            RecipientId = request.RecipientId,
            IsRead = false
        };

        try
        {
            Task addedMessage = _messageService.AddAsync(message, cancellationToken);

            Task sendMessageInRealTime = _domainEventService.Publish(new MessageSentEvent(message, message.RecipientId));

            await Task.WhenAll(addedMessage, sendMessageInRealTime);

            return Result<GetMessageDto>.Success(new GetMessageDto(
                message.Id,
                message.Content,
                message.SenderId,
                message.RecipientId,
                message.Created));
        }
        catch (Exception)
        {
            return Result<GetMessageDto>.Failure("Failed to send message");
        }
    }
}