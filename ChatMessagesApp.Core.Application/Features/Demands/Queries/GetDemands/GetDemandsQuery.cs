using ChatMessagesApp.Core.Application.Features.Demands.Queries;
using ChatMessagesApp.Core.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatMessagesApp.Core.Application.Features;

public class GetDemandsQuery : IRequest<List<GetDemandsDto>>
{
}

public class GetDemandsQueryHandler(IContext context) : IRequestHandler<GetDemandsQuery, List<GetDemandsDto>>
{
    private readonly IContext _demandContext = context;

    public async Task<List<GetDemandsDto>> Handle(GetDemandsQuery request, CancellationToken cancellationToken)
    {
        return await _demandContext.Demands.Select(d => new GetDemandsDto()
        {
            Id = d.Id,
            CreatedByUserId = d.CreatedByUserId,
            Title = d.Title,
            Description = d.Description,
            Created = d.Created,
        }).ToListAsync(cancellationToken);
    }
}
