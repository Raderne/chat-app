using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Application.Models;
using ChatMessagesApp.Core.Application.Responses;
using MediatR;

namespace ChatMessagesApp.Core.Application.Features.Messages.Queries;

public record GetMessagesQuery(Guid DemandId, string UserId) : IRequest<Result<List<GetMessageDto>>>;

public class GetMessagesQueryHandler(IMessageService messageService) : IRequestHandler<GetMessagesQuery, Result<List<GetMessageDto>>>
{
    private readonly IMessageService _messageService = messageService;
    public async Task<Result<List<GetMessageDto>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var messages = await _messageService.GetByDemandIdAsync(request.DemandId);
        if (messages == null)
            return Result<List<GetMessageDto>>.Failure("Messages not found.");

        var result = messages.Select(messages => new GetMessageDto(
            messages.Id,
            messages.SenderId,
            messages.RecipientId,
            messages.Content,
            messages.Created
        )).ToList();

        return Result<List<GetMessageDto>>.Success(result);
    }
}