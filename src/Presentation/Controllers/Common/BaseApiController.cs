using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClubApi.Presentation.Controllers.Common;

[Route("api/[controller]")]
[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected readonly IMediator _mediator;
    public BaseApiController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
}
