using Squares.Application.Common.Interfaces;
using Squares.Shared.Authorization;
using System.Security.Claims;

namespace Squares.Infrastructure.Auth;

public class CurrentUser : ICurrentUser, ICurrentUserInitializer
{
    private ClaimsPrincipal? _user;

    public string? Name => _user?.Identity?.Name;

    private int _userId;

    public int GetUserId()
    {
        return IsAuthenticated()
            ? int.TryParse(_user?.GetUserId(), out int userId) ? userId : 0
            : _userId;
    }

    public string? GetUserEmail()
    {
        return IsAuthenticated() ? _user!.GetEmail() : string.Empty;
    }

    public bool IsAuthenticated() =>
        _user?.Identity?.IsAuthenticated is true;

    public bool IsInRole(string role) =>
        _user?.IsInRole(role) is true;

    public IEnumerable<Claim>? GetUserClaims() =>
        _user?.Claims;

    public string? GetTenant() =>
        IsAuthenticated() ? _user?.GetTenant() : string.Empty;

    public void SetCurrentUser(ClaimsPrincipal user)
    {
        if (_user != null)
        {
            throw new Exception("Method reserved for in-scope initialization");
        }

        _user = user;
    }

    public void SetCurrentUserId(string userId)
    {
        if (_userId != 0)
        {
            throw new Exception("Method reserved for in-scope initialization");
        }

        if (!string.IsNullOrEmpty(userId))
        {
            _userId = int.Parse(userId);
        }
    }
}