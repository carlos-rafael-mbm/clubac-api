using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ClubApi.Presentation.Controllers.Common;
using ClubApi.Application.Commands.Auth.Login;
using ClubApi.Application.Commands.Auth.RefreshToken;

namespace ClubApi.Presentation.Controllers;

public class AuthController : BaseApiController
{
    public AuthController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("Login")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(object), (int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("Refresh-Token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
