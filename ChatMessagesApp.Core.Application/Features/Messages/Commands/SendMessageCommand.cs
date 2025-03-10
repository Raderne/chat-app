using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Domain.DomainEvents;
using ChatMessagesApp.Core.Domain.Entities;
using MediatR;

namespace ChatMessagesApp.Core.Application.Features;

public record SendMessageCommand(Guid DemandId, string SenderId, string Content) : IRequest<MessageDto>;

public class SendMessageCommandHandler(
    IConversationService conversationService,
    IMediator mediator) : IRequestHandler<SendMessageCommand, MessageDto>
{
    private readonly IConversationService _conversationService = conversationService;
    private readonly IMediator _mediator = mediator;

    public async Task<MessageDto> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var conversation = await _conversationService.GetByDemandIdAsync(request.DemandId) ?? new Conversation { DemandId = request.DemandId };

        var message = new Message
        {
            Content = request.Content,
            SenderId = request.SenderId,
            SentAt = DateTime.UtcNow
        };

        // 3. Update conversation
        conversation.Messages.Add(message);
        await _conversationService.UpdateAsync(conversation);

        // 4. Publish domain event
        await _mediator.Publish(new MessageSentEvent(message, conversation.Id));

        return new MessageDto(message.Id, conversation.DemandId, message.SentAt, message.IsRead)
        {
            Content = message.Content,
            SenderId = message.SenderId
        };
    }
}
