using Microsoft.AspNetCore.Http;
using RentAutoApp.Web.Infrastructure.Contracts;
using System.Security.Claims;

namespace RentAutoApp.Web.Infrastructure;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _http;

    public CurrentUserService(IHttpContextAccessor http) => _http = http;

    public string? UserId => _http.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public bool IsAuthenticated => _http.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public bool IsInRole(string role) => _http.HttpContext?.User?.IsInRole(role) ?? false;
}