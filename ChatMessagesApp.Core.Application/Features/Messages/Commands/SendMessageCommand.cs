using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Application.Models;
using ChatMessagesApp.Core.Application.Responses;
using ChatMessagesApp.Core.Domain.Entities;
using ChatMessagesApp.Core.Domain.Events;
using MediatR;

namespace ChatMessagesApp.Core.Application.Features.Messages.Commands;

public record SendMessageCommand(
    Guid DemandId,
    string Content,
    Guid ConversationId) : IRequest<Result<GetMessageDto>>;

public class SendMessageCommandHandler(
    IDomainEventService domainEventService,
    ICurrentUserService currentUserService,
    IConversationService conversationService) : IRequestHandler<SendMessageCommand, Result<GetMessageDto>>
{
    private readonly IDomainEventService _domainEventService = domainEventService;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IConversationService _conversationService = conversationService;

    public async Task<Result<GetMessageDto>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var senderId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(senderId))
            {
                return Result<GetMessageDto>.Failure("User is not logged in");
            }

            var convorsation = await _conversationService.GetByIdAsync(request.ConversationId);

            if (convorsation == null)
                return Result<GetMessageDto>.Failure("Conversation not found");

            var message = new Message()
            {
                DemandId = request.DemandId,
                Content = request.Content,
                SenderId = senderId,
                ConversationId = convorsation.Id,
            };

            convorsation.Messages.Add(message);
            Task addedMessage = _conversationService.UpdateAsync(convorsation, cancellationToken);
            Task sendMessageInRealTime = _domainEventService.Publish(new MessageSentEvent(message, convorsation.ParticipantIds));

            await Task.WhenAll(addedMessage, sendMessageInRealTime);

            return Result<GetMessageDto>.Success(new GetMessageDto(
                message.Id,
                (Guid)message.ConversationId,
                message.SenderId,
                message.Content,
                message.Created,
                message.CreatedBy));
        }
        catch (Exception)
        {
            return Result<GetMessageDto>.Failure("Failed to send message");
        }
    }
}