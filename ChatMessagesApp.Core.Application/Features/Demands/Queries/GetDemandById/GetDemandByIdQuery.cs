using ChatMessagesApp.Core.Application.Features.Demands.Queries;
using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatMessagesApp.Core.Application.Features;

public class GetDemandByIdQuery : IRequest<GetDemandsDto>
{
    public Guid Id { get; set; }
}

public class GetDemandByIdQueryHandler(
    IContext context,
    ICurrentUserService currentUserService) : IRequestHandler<GetDemandByIdQuery, GetDemandsDto>
{
    private readonly IContext _demandContext = context;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<GetDemandsDto> Handle(GetDemandByIdQuery request, CancellationToken cancellationToken)
    {


        var demand = await _demandContext.Demands.Where(d => d.Id == request.Id).Select(d => new GetDemandsDto()
        {
            Id = d.Id,
            ToUserId = d.ToUserId,
            Title = d.Title,
            Description = d.Description,
            Created = d.Created,
            CreatedBy = d.CreatedBy
        }).FirstOrDefaultAsync(cancellationToken);

        if (demand == null)
        {
            throw new NotFoundException(nameof(Demand), request.Id);
        }

        return demand;
    }
}
