using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ClubApi.Presentation.Controllers.Common;
using ClubApi.Application.Commands.Users.RegisterUser;
using ClubApi.Application.Commands.Users.UpdateUser;
using ClubApi.Application.Commands.Users.DeleteUser;
using ClubApi.Application.Queries.Users.Dtos;
using ClubApi.Application.Queries.Users.GetAllUser;
using ClubApi.Application.Queries.Users.GetUserById;
using Microsoft.AspNetCore.Authorization;

namespace ClubApi.Presentation.Controllers;

[Authorize(Policy = "AdminOnly")]
public class UserController : BaseApiController
{
    public UserController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(object), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Get()
    {
        var request = new GetAllUserQuery();
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpGet("id")]
    [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(object), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetById([FromQuery] int id)
    {
        var request = new GetUserByIdQuery(id);
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(object), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create(RegisterUserCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Create), result);
    }

    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(object), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Update(UpdateUserCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(object), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteUserCommand { Id = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
