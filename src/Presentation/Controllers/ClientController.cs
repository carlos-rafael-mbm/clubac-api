using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ClubApi.Presentation.Controllers.Common;
using ClubApi.Application.Commands.Clients.RegisterClient;
using ClubApi.Application.Queries.Clients.Dtos;
using ClubApi.Application.Queries.Clients.GetAllClient;
using ClubApi.Application.Queries.Clients.GetAllClientType;
using Microsoft.AspNetCore.Authorization;

namespace ClubApi.Presentation.Controllers;

[Authorize(Policy = "StaffOnly")]
public class ClientController : BaseApiController
{
    public ClientController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ClientDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(object), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Get()
    {
        var request = new GetAllClientQuery();
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpGet("Types")]
    [ProducesResponseType(typeof(IEnumerable<ClientTypeDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(object), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetClientTypes()
    {
        var request = new GetAllClientTypeQuery();
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(object), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create(RegisterClientCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Create), result);
    }
}
