using ChatMessagesApp.Core.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ChatMessagesApp.Core.Common.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string UserId
    {
        get
        {
            var claimPrincipal = _httpContextAccessor.HttpContext?.User;
            return claimPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }
    }

    public string UserName
    {
        get
        {
            var claimPrincipal = _httpContextAccessor.HttpContext?.User;
            return claimPrincipal?.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
        }
    }

    public string PreferedUserName
    {
        get
        {
            var claimPrincipal = _httpContextAccessor.HttpContext?.User;
            return claimPrincipal?.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty;
        }
    }

    public List<string> Roles
    {
        get
        {
            var claimPrincipal = _httpContextAccessor.HttpContext?.User;
            return claimPrincipal?.FindAll(ClaimTypes.Role).Select(x => x.Value).ToList() ?? [];
        }
    }
}
