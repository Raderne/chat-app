using ChatMessagesApp.Core.Application.Interfaces;
using MediatR;

namespace ChatMessagesApp.Core.Application.Features;

public class GetUsersQuery : IRequest<List<GetUsersDto>>
{
}

public class GetUsersQueryHandler(IIdentityService identityService) : IRequestHandler<GetUsersQuery, List<GetUsersDto>>
{
    private readonly IIdentityService _identityService = identityService;

    public async Task<List<GetUsersDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await _identityService.GetAllUsers(cancellationToken);
    }
}
