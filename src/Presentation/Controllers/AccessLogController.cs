using MediatR;
using Microsoft.AspNetCore.Mvc;
using ClubApi.Presentation.Controllers.Common;
using Microsoft.AspNetCore.Authorization;
using ClubApi.Application.Commands.AccessLogs.Entry;
using ClubApi.Application.Commands.AccessLogs.Exit;
using ClubApi.Application.Queries.AccessLogs.GetAccessLogByFilters;

namespace ClubApi.Presentation.Controllers;

[Authorize(Policy = "StaffOnly")]
public class AccessLogController : BaseApiController
{
    public AccessLogController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("Register-Entry")]
    public async Task<IActionResult> RegisterEntry([FromBody] RegisterEntryCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(RegisterEntry), result);
    }

    [HttpPost("Register-Exit")]
    public async Task<IActionResult> RegisterExit([FromBody] RegisterExitCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet()]
    public async Task<IActionResult> GetAccessLogsByFilters([FromQuery] int? clientId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, CancellationToken cancellationToken)
    {
        var query = new GetAccessLogByFiltersQuery
        {
            ClientId = clientId,
            StartDate = startDate,
            EndDate = endDate
        };

        var logs = await _mediator.Send(query, cancellationToken);
        return Ok(logs);
    }
}
