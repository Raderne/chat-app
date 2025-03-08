using ChatMessagesApp.Core.Application.Features;
using ChatMessagesApp.Core.Application.Features.Demands.Commands.CreateDemand;
using ChatMessagesApp.Core.Application.Features.Demands.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatMessagesApp.Web.FrontOffice.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApplicationController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [Authorize]
    [HttpPost("create-demand")]
    public async Task<ActionResult<CreateDemandDto>> CreateDemand([FromBody] CreateDemandCommand createDemandCommand)
    {
        var demandDto = await _mediator.Send(createDemandCommand);
        return Ok(demandDto);
    }

    [HttpGet("demands")]
    public async Task<ActionResult<List<GetDemandsDto>>> GetDemands()
    {
        var demands = await _mediator.Send(new GetDemandsQuery());
        return Ok(demands);
    }

    [HttpGet("demand/{id}")]
    public async Task<ActionResult<GetDemandsDto>> GetDemand(Guid id)
    {
        var demand = await _mediator.Send(new GetDemandByIdQuery { Id = id });
        return Ok(demand);
    }
}
