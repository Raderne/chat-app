﻿using ChatMessagesApp.Core.Application.Features;
using ChatMessagesApp.Core.Application.Features.Demands.Commands.CreateDemand;
using ChatMessagesApp.Core.Application.Features.Demands.Queries;
using ChatMessagesApp.Core.Application.Features.Messages.Commands;
using ChatMessagesApp.Core.Application.Features.Messages.Queries;
using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatMessagesApp.Web.FrontOffice.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApplicationController(IMediator mediator, ISignalRService signalRService) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ISignalRService _signalRService = signalRService;

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

    [HttpGet("demand/{id}/messages")]
    [Authorize]
    public async Task<ActionResult<List<GetMessageDto>>> GetDemandMessages(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _mediator.Send(new GetMessagesQuery(id, userId!));
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost("send-message")]
    public async Task<ActionResult> SendMessage([FromBody] SendMessageDto messageDto)
    {
        var result = await _mediator.Send(new SendMessageCommand(messageDto.DemandId, messageDto.Content, messageDto.SendToId));

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
