using ClubApi.Domain.Abstractions;

namespace ClubApi.Presentation.Core;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUsername()
    {
        return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? throw new InvalidOperationException("No authenticated user found.");
    }
}
