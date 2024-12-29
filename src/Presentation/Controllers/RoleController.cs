using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ClubApi.Presentation.Controllers.Common;
using ClubApi.Application.Queries.Roles.Dtos;
using ClubApi.Application.Queries.Users.GetAllRole;
using Microsoft.AspNetCore.Authorization;

namespace ClubApi.Presentation.Controllers;

[Authorize(Policy = "AdminOnly")]
public class RoleController : BaseApiController
{
    public RoleController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RoleDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(object), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Get()
    {
        var request = new GetAllRoleQuery();
        var result = await _mediator.Send(request);
        return Ok(result);
    }
}
