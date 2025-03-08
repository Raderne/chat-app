using ChatMessagesApp.Core.Application.Features;
using ChatMessagesApp.Core.Application.Features.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatMessagesApp.Web.FrontOffice.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("login")]
    public async Task<ActionResult<LoginUserDto>> LoginUser(UserCommand userCommand)
    {
        var userDto = await _mediator.Send(userCommand);
        return Ok(userDto);
    }

    [HttpGet]
    public async Task<ActionResult<List<GetUsersDto>>> GetUsers()
    {
        var users = await _mediator.Send(new GetUsersQuery());
        return Ok(users);
    }
}
