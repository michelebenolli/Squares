using System.Security.Claims;

namespace Squares.Application.Common.Interfaces;
public interface ICurrentUser
{
    string? Name { get; }
    int GetUserId();
    string? GetUserEmail();
    string? GetTenant();
    bool IsAuthenticated();
    bool IsInRole(string role);
    IEnumerable<Claim>? GetUserClaims();
}